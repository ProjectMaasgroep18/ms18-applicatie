using Maasgroep.SharedKernel.Interfaces;

namespace Maasgroep.Database
{
    public abstract class WritableRepository<TRecord, TViewModel, TDataModel> : ReadOnlyRepository<TRecord, TViewModel>, IWritableRepository<TRecord, TViewModel, TDataModel>
	where TRecord : GenericRecordActive
    {
        public WritableRepository(MaasgroepContext db) : base(db) {}

		// ABSTRACT METHODS:

		/** Get record from model */
		public abstract TRecord? GetRecord(TDataModel model, TRecord? existingRecord = null);
		// This should create or update a record from a model
		// Should return null if data is not valid or record is not editable

		// END ABSTRACT METHODS


		/** Save a record to the database */
		protected TRecord? SaveToDb(TRecord record)
		{
			Db.Database.BeginTransaction();
			var success = false;
			try {
				Db.Set<TRecord>().Add(record);
				Db.SaveChanges();
				Db.Database.CommitTransaction();
				success = true;
			} catch (Exception) {
				Db.Database.RollbackTransaction();
				Db.ChangeTracker.Clear();
			}
			return success ? record : null;
		}

		/** Get a list of models for a range of records created by a given member */
		public virtual IEnumerable<TViewModel> ListByMember(long memberId, int offset = default, int limit = default) =>
			GetList(item => item.MemberCreatedId == memberId, null, offset, limit).Select(item => GetModel(item)!);

		/** Create a new record and save it to the database */
		public virtual TRecord? Create(TDataModel data, long memberId)
		{
			var record = GetRecord(data);
			if (record == null) // Could not be created from model
				return null;
			
			record.Id = 0;
			record.MemberCreatedId = memberId;
			record.DateTimeCreated = DateTime.UtcNow;

			return SaveToDb(record);
		}
	}
}
