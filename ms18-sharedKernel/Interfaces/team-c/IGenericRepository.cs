namespace Maasgroep.SharedKernel.Interfaces
{
	/** A generic data repository that handles Records and View Models. Data can only be added to the repository, not Edited or Deleted */
	public interface IGenericRepository<TRecord, TModel>
	{
		// Methods to convert between record/model
		TModel? GetModel(long id);
		TModel GetModel(TRecord record);
		TRecord? GetRecord(TModel model, TRecord? existingRecord = default);

		// Methods to query items
		TRecord? GetById(long id);
		IEnumerable<TModel> ListAll(int offset = default, int limit = default);
		IEnumerable<TModel> ListByMember(long memberId, int offset = default, int limit = default);

		// Methods to update the database from models 
		long Create(TModel model, long memberId);
	}
}
