using Maasgroep.Database;
using Maasgroep.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public abstract class ReadOnlyRepositoryController<TRepository, TRecord, TViewModel> : ControllerBase
where TRepository : IReadOnlyRepository<TRecord, TViewModel>
where TRecord: GenericRecordActive
{
	protected readonly TRepository Repository;

	public ReadOnlyRepositoryController(TRepository repository) 
		=> Repository = repository;

    [HttpGet]
    public IActionResult RepositoryGet([FromQuery] int offset = default, [FromQuery] int limit = default)
        => Ok(Repository.ListAll(offset, limit));
    
    [HttpGet("{id}")]
    public IActionResult RepositoryGetById(long id) {
        var repository = Repository.GetById(id);
        if (repository == null)
            return NotFound();
        return Ok(Repository.GetModel(repository));
    }
}