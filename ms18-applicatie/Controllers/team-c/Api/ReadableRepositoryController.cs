using Maasgroep.Database;
using Maasgroep.Exceptions;
using Maasgroep.Interfaces;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;


namespace Maasgroep.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public abstract class ReadableRepositoryController<TRepository, TRecord, TViewModel> : ControllerBase
where TRepository : IReadableRepository<TRecord, TViewModel>
where TRecord: GenericRecordActive
{
    protected readonly TRepository Repository;
    protected readonly IMaasgroepAuthenticationService MaasgroepAuthenticationService;
    public virtual MemberModel? CurrentMember { get => MaasgroepAuthenticationService.GetCurrentMember(HttpContext); }
        
    protected virtual bool AllowList()
        => true; // By default, everyone is allowed to list all items
    
    protected virtual bool AllowView(TRecord? record)
        => true; // By default, everyone can view every single item

    public virtual string ItemName { get => "Item"; }

    public ReadableRepositoryController(TRepository repository, IMaasgroepAuthenticationService maasgroepAuthenticationService)
    { 
       Repository = repository;
       MaasgroepAuthenticationService = maasgroepAuthenticationService;
    }

    [HttpGet]
    public IActionResult RepositoryGet([FromQuery] int offset = default, [FromQuery] int limit = default)
    {
        if (!AllowList())
            NoAccess();
        return Ok(Repository.ListAll(offset, limit));
    }
    
    [HttpGet("{id}")]
    public IActionResult RepositoryGetById(long id)
    {
        var record = Repository.GetById(id);
        if (!AllowView(record))
            NoAccess();
        if (record == null)
            NotFound();
        return Ok(Repository.GetModel(record!));
    }

    protected bool HasPermission(string permission)
        => CurrentMember != null && (CurrentMember.Permissions.Contains("admin") || CurrentMember.Permissions.Contains(permission));

    protected void NoAccess()
    {
        // Throw an Unauthorized or Forbidden error, depending on login state
        if (CurrentMember == null)
            throw new MaasgroepUnauthorized();
        else
            throw new MaasgroepForbidden();
    }

    protected new void NotFound()
        => throw new MaasgroepNotFound($"{ItemName} niet gevonden");
}