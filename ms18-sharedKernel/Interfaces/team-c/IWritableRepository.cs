namespace Maasgroep.SharedKernel.Interfaces
{
	/** A generic data repository that handles Records and View Models. Data can only be added to the repository, not Edited or Deleted */
	public interface IWritableRepository<TRecord, TViewModel, TDataModel> : IReadOnlyRepository<TRecord, TViewModel>
	{
		// Methods to convert between record/model
		TRecord? GetRecord(TDataModel model, TRecord? existingRecord = default);

		// Methods to update the database from models 
		TRecord? Create(TDataModel model, long? memberId);

		// Methods to query items
		IEnumerable<TViewModel> ListByMember(long memberId, int offset = default, int limit = default);
	}
}
