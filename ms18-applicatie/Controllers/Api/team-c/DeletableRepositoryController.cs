using Maasgroep.Database;
using Maasgroep.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public abstract class DeletableRepositoryController<TRepository, TRecord, TModel> : WritableRepositoryController<TRepository, TRecord, TModel>
where TRepository : IDeletableRepository<TRecord, TModel>
where TRecord: GenericRecordActive
{
    protected DeletableRepositoryController(TRepository repository) : base(repository) {}

    [HttpDelete("{id}")]
    public IActionResult RepositoryDelete(long id)
        => Repository.Delete(id, 1) ? NoContent() : BadRequest();
}