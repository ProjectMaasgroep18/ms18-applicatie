using Maasgroep.Database;
using Maasgroep.Exceptions;
using Maasgroep.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public abstract class WritableRepositoryController<TRepository, TRecord, TViewModel, TDataModel> : ReadableRepositoryController<TRepository, TRecord, TViewModel>
where TRepository : IWritableRepository<TRecord, TViewModel, TDataModel>
where TRecord: GenericRecordActive
{
    public WritableRepositoryController(TRepository repository) : base(repository) {}
    
    protected virtual bool AllowCreate(TDataModel data)
        => CurrentMember != null; // By default, all logged-in members are allowed to create items
    
    [HttpPost]
    public IActionResult RepositoryCreate([FromBody] TDataModel data)
    {
        if (!AllowCreate(data))
            NoAccess();
        var record = Repository.Create(data, CurrentMember?.Id);
        if (record == null)
            NotWritable();
        return Created($"/api/v1/{RouteData.Values["controller"]}/{record!.Id}", Repository.GetModel(record));
    }

    protected void NotWritable()
        => throw new MaasgroepBadRequest($"{ItemName} kon niet worden aangemaakt");
}