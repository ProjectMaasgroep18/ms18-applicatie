namespace Maasgroep.SharedKernel.Interfaces
{
    /** An generic data repository that handles Records and View Models. These records can also be deleted. */
    public interface IDeletableRepository<TRecord, TViewModel, TDataModel> : IWritableRepository<TRecord, TViewModel, TDataModel>
    {
        // Methods to query items
        TRecord? GetById(long id, bool includeDeleted = default);
        IEnumerable<TViewModel> ListAll(int offset = default, int limit = default, bool includeDeleted = default);
        IEnumerable<TViewModel> ListByMember(long memberId, int offset = default, int limit = default, bool includeDeleted = default);

        // Methods to update the database from models 
        bool Delete(TRecord record, long? memberId);
    }
}
