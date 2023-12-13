using Maasgroep.SharedKernel.Interfaces;

namespace Maasgroep.Database
{
    public abstract class WritableRepository<TRecord, TModel> : ReadOnlyRepository<TRecord, TModel>, IWritableRepository<TRecord, TModel>
	where TRecord : GenericRecordActive
    {
        public WritableRepository(MaasgroepContext db) : base(db) {}

		/** Save a record to the database */
		protected TRecord SaveToDb(TRecord record)
		{
			_db.Database.BeginTransaction();
			var success = false;
			try {
				_db.Set<TRecord>().Add(record);
				_db.SaveChanges();
				_db.Database.CommitTransaction();
				success = true;
			} catch (Exception) {
				_db.Database.RollbackTransaction();
				_db.ChangeTracker.Clear();
			}
			return success ? record : null;
		}

		/** Get a list of models for a range of records created by a given member */
		public virtual IEnumerable<TModel> ListByMember(long memberId, int offset = default, int limit = default) =>
			GetList(item => item.MemberCreatedId == memberId, null, offset, limit).Select(item => GetModel(item)!);

		/** Create a new record and save it to the database */
		public virtual TRecord? Create(TModel model, long memberId)
		{
			var record = GetRecord(model);
			if (record == null) // Could not be created from model
				return null;
			
			record.Id = 0;
			record.MemberCreatedId = memberId;
			record.DateTimeCreated = DateTime.UtcNow;

			return SaveToDb(record);
		}
	}
}
