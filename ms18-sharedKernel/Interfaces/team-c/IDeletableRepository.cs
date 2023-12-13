namespace Maasgroep.SharedKernel.Interfaces
{
    /** An generic data repository that handles Records and View Models. These records can also be deleted. */
	public interface IDeletableRepository<TRecord, TModel> : IWritableRepository<TRecord, TModel>
	{
		// Methods to query items
		IEnumerable<TModel> ListAll(int offset = default, int limit = default, bool includeDeleted = default);
		IEnumerable<TModel> ListByMember(long memberId, int offset = default, int limit = default, bool includeDeleted = default);

		// Methods to update the database from models 
    	bool Delete(long id, long memberId);
	}
}
