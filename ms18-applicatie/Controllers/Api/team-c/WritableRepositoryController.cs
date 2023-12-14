using Maasgroep.Database;
using Maasgroep.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public abstract class WritableRepositoryController<TRepository, TRecord, TModel> : ReadOnlyRepositoryController<TRepository, TRecord, TModel>
where TRepository : IWritableRepository<TRecord, TModel>
where TRecord: GenericRecordActive
{
	public WritableRepositoryController(TRepository repository) : base(repository) {}
    
    [HttpPost]
    public IActionResult RepositoryCreate([FromBody] TModel model)
    {
        var repository = Repository.Create(model, 1);
        if (repository == null)
            return BadRequest(model);
        return Created($"/api/v1/{RouteData.Values["controller"]}/{repository.Id}", Repository.GetModel(repository));
    }
}