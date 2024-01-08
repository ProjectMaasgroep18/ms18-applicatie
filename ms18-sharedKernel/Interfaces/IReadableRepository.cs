namespace Maasgroep.SharedKernel.Interfaces
{
    /** A generic data repository that handles Records and View Models. Data can only be read, not Created, Edited or Deleted */
    public interface IReadableRepository<TRecord, TViewModel>
    {
        // Methods to convert between record/model
        TViewModel? GetModel(long id);
        TViewModel GetModel(TRecord record);

        // Methods to query items
        bool Exists(long id);
        TRecord? GetById(long id, bool includeDeleted = default);
        IEnumerable<TViewModel> ListAll(int offset = default, int limit = default);
    }
}
