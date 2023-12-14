using Maasgroep.Database;
using Maasgroep.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public abstract class WritableRepositoryController<TRepository, TRecord, TViewModel, TDataModel> : ReadOnlyRepositoryController<TRepository, TRecord, TViewModel>
where TRepository : IWritableRepository<TRecord, TViewModel, TDataModel>
where TRecord: GenericRecordActive
{
	public WritableRepositoryController(TRepository repository) : base(repository) {}
    
    [HttpPost]
    public IActionResult RepositoryCreate([FromBody] TDataModel data)
    {
        var repository = Repository.Create(data, 1);
        if (repository == null)
            return BadRequest(data);
        return Created($"/api/v1/{RouteData.Values["controller"]}/{repository.Id}", Repository.GetModel(repository));
    }
}