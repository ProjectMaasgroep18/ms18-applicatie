namespace Maasgroep.SharedKernel.Interfaces
{
	/** A generic data repository that handles Records and View Models. Data can only be read, not Created, Edited or Deleted */
	public interface IReadOnlyRepository<TRecord, TModel>
	{
		// Methods to convert between record/model
		TModel? GetModel(long id);
		TModel GetModel(TRecord record);
		TRecord? GetRecord(TModel model, TRecord? existingRecord = default);

		// Methods to query items
		TRecord? GetById(long id);
		IEnumerable<TModel> ListAll(int offset = default, int limit = default);
	}
}
