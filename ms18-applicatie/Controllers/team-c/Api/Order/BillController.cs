using Maasgroep.Database.Orders;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;
using Microsoft.AspNetCore.Mvc;
using Maasgroep.Interfaces;

namespace Maasgroep.Controllers.Api;

public class BillController : DeletableRepositoryController<IBillRepository, Bill, BillModel, BillData>
{
    public override string ItemName { get => "Bestelling"; }
    public BillController(IBillRepository repository, IMaasgroepAuthenticationService maasgroepAuthenticationService) : base(repository, maasgroepAuthenticationService) {}
    
    protected override bool AllowCreate(BillData Bill)
        => true; // Everyone can place orders, even not logged in (= guest)

    protected override bool AllowList()
        => HasPermission("order.view");

    protected override bool AllowView(Bill? Bill)
        => HasPermission("order.view") || (CurrentMember != null && Bill?.MemberCreatedId == CurrentMember.Id);

    protected override bool AllowDelete(Bill? Bill)
        => HasPermission("admin") || (CurrentMember != null && Bill?.MemberCreatedId == CurrentMember.Id);
}