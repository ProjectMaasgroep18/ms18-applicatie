using Maasgroep.SharedKernel.Interfaces;

namespace Maasgroep.Database
{
    public abstract class GenericRepository<TRecord, TModel> : IGenericRepository<TRecord, TModel>
	where TRecord : GenericRecordActive
    {
        protected readonly MaasgroepContext _db;

        public GenericRepository(MaasgroepContext db) => _db = db;

		// Default number of items per page
		const int defaultLimit = 100;

		// ABSTRACT METHODS:

		/** Get model from record */
		public abstract TModel GetModel(TRecord record);
		// This should create a model based on the given record

		/** Get record from model */
		public abstract TRecord? GetRecord(TModel model, TRecord? existingRecord = null);
		// This should create or update a record from a model
		// Should return null if data is not valid or record is not editable

		// END ABSTRACT METHODS


		/** Get list of records based on parameters */
		protected virtual IEnumerable<TRecord> GetList(Func<TRecord, bool>? filter = default, Func<TRecord, int>? priority = default, int offset = default, int limit = default)
		{
			if (limit == default)
				limit = defaultLimit;

			return _db.Set<TRecord>()
				.Where(item => filter == null || filter(item))
				.OrderByDescending(item => priority == null ? 0 : priority(item))
				.ThenByDescending(item => item.DateTimeCreated)
				.Skip(offset)
				.Take(limit)
				.ToList();
		}

		/** Get record by ID */
		public virtual TRecord? GetById(long id) => _db.Set<TRecord>().FirstOrDefault(item => item.Id == id);

		/** Save a record to the database */
		protected bool SaveToDb(TRecord record)
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
			return success;
		}

		/** Get model by ID */
		public TModel? GetModel(long id)
		{
			var record = GetById(id);

			if (record == null) 
				return default;

			return GetModel(record);
		}

		/** Get a list of models for a range of records */
		public virtual IEnumerable<TModel> ListAll(int offset = default, int limit = default) =>
			GetList(null, null, offset, limit).Select(item => GetModel(item)!);

		/** Get a list of models for a range of records created by a given member */
		public virtual IEnumerable<TModel> ListByMember(long memberId, int offset = default, int limit = default) =>
			GetList(item => item.MemberCreatedId == memberId, null, offset, limit).Select(item => GetModel(item)!);

		/** Create a new record and save it to the database */
		public virtual long Create(TModel model, long memberId)
		{
			var record = GetRecord(model);
			if (record == null) // Could not be created from model
				return 0;
			
			record.Id = 0;
			record.MemberCreatedId = memberId;
			record.DateTimeCreated = DateTime.UtcNow;

			return SaveToDb(record) ? record.Id : 0;
		}
	}
}
