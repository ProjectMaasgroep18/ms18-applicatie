using Maasgroep.Database;
using Maasgroep.Exceptions;
using Maasgroep.Services;
using Maasgroep.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public abstract class EditableRepositoryController<TRepository, TRecord, TViewModel, TDataModel, THistory> : DeletableRepositoryController<TRepository, TRecord, TViewModel, TDataModel>
where TRepository : IEditableRepository<TRecord, TViewModel, TDataModel, THistory>
where TRecord: GenericRecordActive
where THistory: GenericRecordHistory
{
    protected EditableRepositoryController(TRepository repository) : base(repository) {}

    protected virtual bool AllowEdit(TRecord record)
        => AllowDelete(record); // By default, same as delete (i.e., only allowed to delete their own items)

    [HttpPut("{id}")]
    public IActionResult RepositoryUpdate(long id, [FromBody] TDataModel data)
    {
        var record = Repository.GetById(id);
        if (record == null || !AllowView(record))
            throw new MaasgroepNotFound($"{ItemName} niet gevonden");
        if (!AllowEdit(record))
            NoAccess();
        if (Repository.Update(record, data, CurrentMember?.Id) == null)
            throw new MaasgroepNotFound($"{ItemName} kon niet worden opgeslagen");
        return NoContent();
    }
}