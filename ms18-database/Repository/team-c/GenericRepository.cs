using Maasgroep.SharedKernel.Interfaces;

namespace Maasgroep.Database
{

    public abstract class GenericRepository<TRecord, TModel, THistory> : IGenericRepository<TRecord, TModel, THistory>
	where TRecord : GenericRecordActive
	where THistory : GenericRecordHistory
    {
        protected readonly MaasgroepContext _db;

        public GenericRepository(MaasgroepContext db) => _db = db;


		// ABSTRACT METHODS:

		/** Get model from record */
		public abstract TModel GetModel(TRecord record);
		// This should create a model based on the given record

		/** Get record from model */
		public abstract TRecord? GetRecord(TModel model, TRecord? existingRecord = null);
		// This should create or update a record from a model
		// Should return null if data is not valid or record is not editable

		/** Get history record from record */
		public abstract THistory GetHistory(TRecord record);
		// This should create a history record from an existing record

		// END ABSTRACT METHODS


		/** Get list of records based on parameters */
		protected virtual IEnumerable<TRecord> GetList(Func<TRecord, bool>? filter = null, Func<TRecord, int>? priority = null, int offset = 0, int limit = 100, bool includeDeleted = false)
		{
			return _db.Set<TRecord>()
				.Where(item => (filter == null || filter(item)) && (item.DateTimeDeleted == null || includeDeleted))
				.OrderByDescending(item => priority == null ? 0 : priority(item))
				.ThenByDescending(item => item.DateTimeCreated)
				.Skip(offset)
				.Take(limit)
				.ToList();
		}

		/** Get record by ID */
		public virtual TRecord? GetById(long id) => _db.Set<TRecord>().FirstOrDefault(item => item.Id == id);

		/** Save a record to the database */
		protected bool SaveToDb(TRecord record, THistory? historyRecord = null)
		{
			_db.Database.BeginTransaction();
			var success = false;
			try {
				_db.Set<TRecord>().Add(record);
				if (historyRecord != null)
					_db.Set<THistory>().Add(historyRecord);
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
		public virtual IEnumerable<TModel> ListAll(int offset = 0, int limit = 100, bool includeDeleted = false) =>
			GetList(null, null, offset, limit, includeDeleted).Select(item => GetModel(item)!).ToList();

		/** Get a list of models for a range of records created by a given member */
		public virtual IEnumerable<TModel> ListByMember(long memberId, int offset = 0, int limit = 100, bool includeDeleted = false) =>
			GetList(item => item.MemberCreatedId == memberId, null, offset, limit, includeDeleted).Select(item => GetModel(item)!).ToList();

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

		/** Update an existing record in the database */
		public virtual bool Update(long id, TModel model, long memberId)
		{
			var record = GetById(id);
			if (record == null)
				return false; // Does not exist

			var history = GetHistory(record);
			record = GetRecord(model, record);

			if (record == null)
				return false; // Not editable or invalid data

			history.MemberCreatedId = record.MemberCreatedId;
			history.MemberModifiedId = record.MemberModifiedId;
			history.MemberDeletedId = record.MemberDeletedId;
			history.DateTimeCreated = record.DateTimeCreated;
			history.DateTimeModified = record.DateTimeModified;
			history.DateTimeDeleted = record.DateTimeDeleted;

			record.MemberModifiedId = memberId;
			record.DateTimeModified = DateTime.UtcNow;

			return SaveToDb(record, history);
		}

		/** Delete an existing record from the database (set DateTimeDeleted and keep a history) */
		public virtual bool Delete(long id, long memberId)
		{
			var record = GetById(id);

			if (record == null)
				return false; // Does not exist

			var history = GetHistory(record);

			history.MemberCreatedId = record.MemberCreatedId;
			history.MemberModifiedId = record.MemberModifiedId;
			history.MemberDeletedId = record.MemberDeletedId;
			history.DateTimeCreated = record.DateTimeCreated;
			history.DateTimeModified = record.DateTimeModified;
			history.DateTimeDeleted = record.DateTimeDeleted;

			record.MemberDeletedId = memberId;
			record.DateTimeDeleted = DateTime.UtcNow;

			return SaveToDb(record, history);
		}

	}
}
