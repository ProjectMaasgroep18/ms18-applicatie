using Maasgroep.SharedKernel.Interfaces;

namespace Maasgroep.Database
{
    public abstract class EditableRepository<TRecord, TViewModel, TDataModel, THistory> : DeletableRepository<TRecord, TViewModel, TDataModel>, IEditableRepository<TRecord, TViewModel, TDataModel, THistory>
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
		protected TRecord? SaveToDb(TRecord record, THistory history)
		{
			Db.Database.BeginTransaction();
			var success = false;
			try {
				Db.Set<TRecord>().Add(record);
				Db.Set<THistory>().Add(history);
				Db.SaveChanges();
				Db.Database.CommitTransaction();
				success = true;
			} catch (Exception) {
				Db.Database.RollbackTransaction();
				Db.ChangeTracker.Clear();
			}
			return success ? record : null;
		}

		/** Update an existing record in the database */
		public virtual TRecord? Update(long id, TDataModel model, long memberId)
		{
			var record = GetById(id);
			if (record == null)
				return null; // Does not exist

			var history = GetHistory(record);
			record = GetRecord(model, record);

			if (record == null)
				return null; // Not editable or invalid data

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