using Maasgroep.Database;
using Maasgroep.Exceptions;
using Maasgroep.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public abstract class DeletableRepositoryController<TRepository, TRecord, TViewModel, TDataModel> : WritableRepositoryController<TRepository, TRecord, TViewModel, TDataModel>
where TRepository : IDeletableRepository<TRecord, TViewModel, TDataModel>
where TRecord: GenericRecordActive
{
    protected DeletableRepositoryController(TRepository repository) : base(repository) {}
    protected virtual bool AllowDelete(TRecord? record)
        => CurrentMember != null && record?.MemberCreatedId == CurrentMember.Id; // By default, logged-in members are only allowed to delete their own items

    [HttpDelete("{id}")]
    public IActionResult RepositoryDelete(long id)
    {
        var record = Repository.GetById(id);
        if (record == null || !AllowView(record))
            throw new MaasgroepNotFound($"{ItemName} niet gevonden");
        if (!AllowDelete(record))
            NoAccess();
        if (!Repository.Delete(record, CurrentMember?.Id))
            throw new MaasgroepNotFound($"{ItemName} kon niet worden verwijderd");
        return NoContent();
    }
}