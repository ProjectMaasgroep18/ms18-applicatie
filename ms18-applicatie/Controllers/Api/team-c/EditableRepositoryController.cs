using Maasgroep.Database;
using Maasgroep.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public abstract class EditableRepositoryController<TRepository, TRecord, TModel, THistory> : DeletableRepositoryController<TRepository, TRecord, TModel>
where TRepository : IEditableRepository<TRecord, TModel, THistory>
where TRecord: GenericRecordActive
where THistory: GenericRecordHistory
{
    protected EditableRepositoryController(TRepository repository) : base(repository) {}

    [HttpPut("{id}")]
    public IActionResult RepositoryUpdate(long id, [FromBody] TModel model)
        => Repository.Update(id, model, 1) == null ? BadRequest() : Ok();
}