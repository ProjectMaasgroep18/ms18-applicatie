using Maasgroep.Database.Admin;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;
using Maasgroep.SharedKernel.DataModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public class UserController : DeletableRepositoryController<IMemberRepository, Member, MemberModel, MemberData>
{
    protected readonly IReceiptRepository Receipts;

    public UserController(IMemberRepository repository, IReceiptRepository receipts) : base(repository)
        => Receipts = receipts;

    [HttpGet("{id}/Receipt")]
    public IActionResult UserGetReceipts(long id, [FromQuery] int offset = default, [FromQuery] int limit = default, [FromQuery] bool includeDeleted = default)
        => Ok(Receipts.ListByMember(id, offset, limit, includeDeleted));

    [HttpGet("Current")]
    public IActionResult CurrentUser()
        => Ok(Repository.GetModel(Repository.GetById(CurrentMemberId) ?? throw new Exceptions.MaasgroepUnauthorized()));
}