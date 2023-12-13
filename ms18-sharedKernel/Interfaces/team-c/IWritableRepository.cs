namespace Maasgroep.SharedKernel.Interfaces
{
	/** A generic data repository that handles Records and View Models. Data can only be added to the repository, not Edited or Deleted */
	public interface IWritableRepository<TRecord, TModel> : IReadOnlyRepository<TRecord, TModel>
	{
		// Methods to update the database from models 
		TRecord? Create(TModel model, long memberId);

		// Methods to query items
		IEnumerable<TModel> ListByMember(long memberId, int offset = default, int limit = default);
	}
}
