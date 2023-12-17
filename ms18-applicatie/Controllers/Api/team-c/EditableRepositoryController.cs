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
    {
        if (!Repository.Exists(id))
            throw new Exceptions.MaasgroepNotFound($"{ItemName} niet gevonden");
        if (Repository.Update(id, data, CurrentMemberId) == null)
            throw new Exceptions.MaasgroepNotFound($"{ItemName} kon niet worden opgeslagen");
        return NoContent();
    }
}