using Maasgroep.SharedKernel.Interfaces;

namespace Maasgroep.Database
{
    public abstract class ReadableRepository<TRecord, TViewModel> : IReadableRepository<TRecord, TViewModel>
	where TRecord : GenericRecordActive
    {
        protected readonly MaasgroepContext Db;

        public ReadableRepository(MaasgroepContext db) => Db = db;

		// Default number of items per page
		const int defaultLimit = 100;

		// ABSTRACT METHODS:

		/** Get model from record */
		public abstract TViewModel GetModel(TRecord record);
		// This should create a model based on the given record

		// END ABSTRACT METHODS


		/** Get list of records based on parameters */
		protected virtual IEnumerable<TRecord> GetList(Func<TRecord, bool>? filter = default, Func<TRecord, int>? priority = default, int offset = default, int limit = default)
		{
			if (limit == default)
				limit = defaultLimit;

			return Db.Set<TRecord>()
				.Where(filter ?? (item => true))
				.OrderByDescending(priority ?? (item => 0))
				.ThenByDescending(item => item.DateTimeCreated)
				.Skip(offset)
				.Take(limit)
				.ToList();
		}

		/** See if record with ID exists */
		public virtual bool Exists(long id) => Db.Set<TRecord>().Select(item => item.Id == id).Any();

		/** Get record by ID */
		public virtual TRecord? GetById(long id) => Db.Set<TRecord>().FirstOrDefault(item => item.Id == id);

		/** Get model by ID */
		public TViewModel? GetModel(long id)
		{
			var record = GetById(id);

			if (record == null) 
				return default;

			return GetModel(record);
		}

		/** Get a list of models for a range of records */
		public virtual IEnumerable<TViewModel> ListAll(int offset = default, int limit = default) =>
			GetList(null, null, offset, limit).Select(item => GetModel(item)!);
	}
}
