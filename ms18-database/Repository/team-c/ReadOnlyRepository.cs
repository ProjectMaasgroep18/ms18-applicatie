using Maasgroep.SharedKernel.Interfaces;

namespace Maasgroep.Database
{
    public abstract class ReadOnlyRepository<TRecord, TModel> : IReadOnlyRepository<TRecord, TModel>
	where TRecord : GenericRecordActive
    {
        protected readonly MaasgroepContext _db;

        public ReadOnlyRepository(MaasgroepContext db) => _db = db;

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

		/** See if record with ID exists */
		public virtual bool Exists(long id) => _db.Set<TRecord>().Select(item => item.Id == id).Any();

		/** Get record by ID */
		public virtual TRecord? GetById(long id) => _db.Set<TRecord>().FirstOrDefault(item => item.Id == id);

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
	}
}
