namespace Maasgroep.SharedKernel.Interfaces
{
	public interface IGenericRepository<TRecord, TModel, THistory>
	{
		// Methods to convert between record/model/history
		TModel? GetModel(long id);
		TModel GetModel(TRecord record);
		TRecord? GetRecord(TModel model, TRecord? existingRecord = default);
		THistory GetHistory(TRecord record);

		// Methods to query items
		TRecord? GetById(long id);
		IEnumerable<TModel> ListAll(int offset, int limit, bool includeDeleted = default);
		IEnumerable<TModel> ListByMember(long memberId, int offset = default, int limit = default, bool includeDeleted = default);

		// Methods to update the database from models 
		long Create(TModel model, long memberId);
		bool Update(long id, TModel model, long memberId);
		bool Delete(long id, long memberId);
	}
}
