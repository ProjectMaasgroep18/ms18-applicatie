using Maasgroep.Database;
using Maasgroep.Exceptions;
using Maasgroep.Interfaces;
using Maasgroep.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public abstract class DeletableRepositoryController<TRepository, TRecord, TViewModel, TDataModel> : WritableRepositoryController<TRepository, TRecord, TViewModel, TDataModel>
where TRepository : IDeletableRepository<TRecord, TViewModel, TDataModel>
where TRecord: GenericRecordActive
{
    protected DeletableRepositoryController(TRepository repository, IMaasgroepAuthenticationService maasgroepAuthenticationService) : base(repository, maasgroepAuthenticationService) { }

    protected virtual bool AllowDelete(TRecord? record)
        => AllowList() && record?.MemberCreatedId == CurrentMember?.Id; // By default, logged-in members are only allowed to delete their own items

    [HttpDelete("{id}")]
    public IActionResult RepositoryDelete(long id)
    {
        var record = Repository.GetById(id);
        if (record == null || !AllowView(record))
            throw new MaasgroepNotFound($"{ItemName} niet gevonden");
        if (!AllowDelete(record))
            NoAccess();
        if (!Repository.Delete(record, CurrentMember?.Id))
            throw new MaasgroepBadRequest($"{ItemName} kon niet worden verwijderd");
        return NoContent();
    }
}