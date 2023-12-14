using Maasgroep.Database;
using Maasgroep.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public abstract class EditableRepositoryController<TRepository, TRecord, TViewModel, TDataModel, THistory> : DeletableRepositoryController<TRepository, TRecord, TViewModel, TDataModel>
where TRepository : IEditableRepository<TRecord, TViewModel, TDataModel, THistory>
where TRecord: GenericRecordActive
where THistory: GenericRecordHistory
{
    protected EditableRepositoryController(TRepository repository) : base(repository) {}

    [HttpPut("{id}")]
    public IActionResult RepositoryUpdate(long id, [FromBody] TDataModel data)
        => Repository.Update(id, data, 1) == null ? BadRequest() : Ok();
}