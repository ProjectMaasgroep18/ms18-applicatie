using Maasgroep.SharedKernel.Interfaces;

namespace Maasgroep.Database
{
    public abstract class DeletableRepository<TRecord, TModel> : WritableRepository<TRecord, TModel>, IDeletableRepository<TRecord, TModel>
	where TRecord : GenericRecordActive
    {
        public DeletableRepository(MaasgroepContext db) : base(db) {}

		/** Delete an existing record from the database (set DateTimeDeleted, don't actually delete) */
		public virtual bool Delete(long id, long memberId)
		{
			var record = GetById(id);

			if (record == null)
				return false; // Does not exist

			record.MemberDeletedId = memberId;
			record.DateTimeDeleted = DateTime.UtcNow;

			return SaveToDb(record);
		}

        /** Get list of records based on parameters (hide deleted records by default) */
		protected override IEnumerable<TRecord> GetList(Func<TRecord, bool>? filter = default, Func<TRecord, int>? priority = default, int offset = default, int limit = default)
            => GetList(filter, priority, offset, limit, false);

        /** Get list of records based on parameters (set includeDeleted to true to include records that were marked as deleted) */
		protected virtual IEnumerable<TRecord> GetList(Func<TRecord, bool>? filter = default, Func<TRecord, int>? priority = default, int offset = default, int limit = default, bool includeDeleted = default)
            => base.GetList(item => (includeDeleted || item.DateTimeDeleted == null) && (filter == null || filter(item)), priority, offset, limit);

		/** Get a list of models for a range of records (set includeDeleted to true to include records that were marked as deleted) */
        public IEnumerable<TModel> ListAll(int offset = default, int limit = default, bool includeDeleted = default)
            => GetList(null, null, offset, limit, includeDeleted).Select(item => GetModel(item)!);
        
        /** Get a list of models for a range of records created by a given member (set includeDeleted to true to include records that were marked as deleted) */
        public IEnumerable<TModel> ListByMember(long memberId, int offset = default, int limit = default, bool includeDeleted = default)
            => GetList(item => item.MemberCreatedId == memberId, null, offset, limit, includeDeleted).Select(item => GetModel(item)!);
    }
}