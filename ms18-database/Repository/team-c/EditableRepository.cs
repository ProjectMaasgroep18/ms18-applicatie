using Maasgroep.SharedKernel.Interfaces;

namespace Maasgroep.Database
{
    public abstract class EditableRepository<TRecord, TModel, THistory> : DeletableRepository<TRecord, TModel>, IEditableRepository<TRecord, TModel, THistory>
	where TRecord : GenericRecordActive
	where THistory : GenericRecordHistory
    {
        public EditableRepository(MaasgroepContext db) : base(db) {}


		// ABSTRACT METHODS:

		/** Get history record from record */
		public abstract THistory GetHistory(TRecord record);
		// This should create a history record from an existing record

		// END ABSTRACT METHODS


        /** Save a record to the database (with history) */
		protected bool SaveToDb(TRecord record, THistory history)
		{
			_db.Database.BeginTransaction();
			var success = false;
			try {
				_db.Set<TRecord>().Add(record);
				_db.Set<THistory>().Add(history);
				_db.SaveChanges();
				_db.Database.CommitTransaction();
				success = true;
			} catch (Exception) {
				_db.Database.RollbackTransaction();
				_db.ChangeTracker.Clear();
			}
			return success;
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
    }
}