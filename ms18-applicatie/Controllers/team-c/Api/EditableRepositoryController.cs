using Maasgroep.Database;
using Maasgroep.Exceptions;
using Maasgroep.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public abstract class EditableRepositoryController<TRepository, TRecord, TViewModel, TDataModel, THistory> : DeletableRepositoryController<TRepository, TRecord, TViewModel, TDataModel>
where TRepository : IEditableRepository<TRecord, TViewModel, TDataModel, THistory>
where TRecord: GenericRecordActive
where THistory: GenericRecordHistory
{
    protected EditableRepositoryController(TRepository repository) : base(repository) {}

    protected virtual bool AllowEdit(TRecord record, TDataModel data)
        => AllowDelete(record) && AllowCreate(data); // By default, same as delete + create (i.e., only allowed to edit/delete their own items)

    [HttpPut("{id}")]
    public IActionResult RepositoryUpdate(long id, [FromBody] TDataModel data)
    {
        var record = Repository.GetById(id);
        if (record == null || !AllowView(record))
            NotFound();
        if (!AllowEdit(record!, data))
            NoAccess();
        if (Repository.Update(record!, data, CurrentMember?.Id) == null)
            throw new MaasgroepBadRequest($"{ItemName} kon niet worden opgeslagen");
        return NoContent();
    }
}