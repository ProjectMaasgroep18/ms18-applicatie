namespace Maasgroep.SharedKernel.Interfaces
{
    /** An editable generic data repository that handles Records and View Models. History is kept in specialized History records. */
	public interface IEditableRepository<TRecord, TModel, THistory> : IDeletableRepository<TRecord, TModel>
	{
		// Methods to convert between record/history
		THistory GetHistory(TRecord record);

		// Methods to update the database from models 
		TRecord? Update(long id, TModel model, long memberId);
	}
}
