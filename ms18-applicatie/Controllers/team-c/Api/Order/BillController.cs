using Maasgroep.Database.Orders;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public class BillController : DeletableRepositoryController<IBillRepository, Bill, BillModel, BillData>
{
    public override string ItemName { get => "Bestelling"; }
    public BillController(IBillRepository repository) : base(repository) {}
    
    protected override bool AllowCreate(BillData Bill)
        => HasPermission("order");

    protected override bool AllowList()
        => HasPermission("order.view");

    protected override bool AllowView(Bill? Bill)
        => HasPermission("order.view") || (CurrentMember != null && Bill?.MemberCreatedId == CurrentMember.Id);

    protected override bool AllowDelete(Bill? Bill)
        => HasPermission("admin") || (CurrentMember != null && Bill?.MemberCreatedId == CurrentMember.Id);

    [HttpGet("Total")]
    public IActionResult BillGetTotal()
    {   
        if (!HasPermission("order.view"))
            NoAccess();
        return Ok(Repository.GetTotal());
    }
}