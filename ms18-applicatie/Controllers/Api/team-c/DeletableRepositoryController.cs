using Maasgroep.Database;
using Maasgroep.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public abstract class DeletableRepositoryController<TRepository, TRecord, TViewModel, TDataModel> : WritableRepositoryController<TRepository, TRecord, TViewModel, TDataModel>
where TRepository : IDeletableRepository<TRecord, TViewModel, TDataModel>
where TRecord: GenericRecordActive
{
    protected DeletableRepositoryController(TRepository repository) : base(repository) {}

    [HttpDelete("{id}")]
    public IActionResult RepositoryDelete(long id)
    {
        if (!Repository.Exists(id))
            throw new Exceptions.MaasgroepNotFound($"{ItemName} niet gevonden");
        if (!Repository.Delete(id, CurrentMemberId))
            throw new Exceptions.MaasgroepNotFound($"{ItemName} kon niet worden verwijderd");
        return NoContent();
    }
}