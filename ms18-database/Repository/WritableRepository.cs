using Maasgroep.SharedKernel.Interfaces;

namespace Maasgroep.Database
{
    public abstract class WritableRepository<TRecord, TViewModel, TDataModel> : ReadableRepository<TRecord, TViewModel>, IWritableRepository<TRecord, TViewModel, TDataModel>
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
		protected TRecord? SaveToDb(TRecord record, Action<MaasgroepContext>? ExtraChanges = null)
		{
			Console.WriteLine($"Start DB transaction in {this}");
			Db.Database.BeginTransaction();
			var success = false;
			try {
				GetSaveAction(record)?.Invoke(Db);
                ExtraChanges?.Invoke(Db);
                Db.SaveChanges();
				Db.Database.CommitTransaction();
				Console.WriteLine($"Commit DB transaction in {this}");
				success = true;
			} catch (Exception e) {
				Console.WriteLine(e.Message);
				var exception = e.InnerException;
				while (exception != null) {
					Console.WriteLine(" -" + exception.Message);
					exception = exception.InnerException;
				}
				Db.Database.RollbackTransaction();
				Db.ChangeTracker.Clear();
				Console.WriteLine($"Rollback DB transaction in {this}");
			}
			if (success)
				GetAfterSaveAction(record)?.Invoke();
			return success ? record : null;
		}

		/** Get a list of models for a range of records created by a given member */
		public virtual IEnumerable<TViewModel> ListByMember(long memberId, int offset = default, int limit = default) =>
			GetList(item => item.MemberCreatedId == memberId, null, offset, limit).Select(item => GetModel(item)!);

		/** Create a new record and save it to the database */
		public virtual TRecord? Create(TDataModel data, long? memberId)
		{
			var record = GetRecord(data);
			if (record == null) // Could not be created from model
				return null;
			
			record.Id = 0;
			record.MemberCreatedId = memberId;
			record.DateTimeCreated = DateTime.UtcNow;

			return SaveToDb(record);
		}

		/** Get action that will save record changes to DB */
		public virtual Action<MaasgroepContext> GetSaveAction(TRecord record)
		{
			return (MaasgroepContext db) => {
				if (record.Id == 0) {
					Console.WriteLine($"INSERT new record in {this}");
					db.Set<TRecord>().Add(record);
				} else {
					Console.WriteLine($"UPDATE record {record.Id} in {this}");
					db.Set<TRecord>().Update(record);
				}
			};
		}

		/** Get action that will be executed after any save to DB occurs */
		public virtual Action GetAfterSaveAction(TRecord record)
			=> () => {}; // Nothing here (yet)
	}
}
