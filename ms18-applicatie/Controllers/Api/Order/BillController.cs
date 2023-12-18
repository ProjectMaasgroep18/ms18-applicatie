using Maasgroep.Database.Orders;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Orders;
using Maasgroep.SharedKernel.DataModels.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public class BillController : EditableRepositoryController<IBillRepository, Bill, BillModel, BillData, BillHistory>
{
    //protected IStockRepository Stock;
    // protected IBillLinesRepository BillLines;
    public override string ItemName { get => "Bestelling"; }
    public BillController(IBillRepository repository) : base(repository) {}
        // => BillLines
    
    protected override bool AllowCreate(BillData Bill)
        => true; // Everyone can place orders, even not logged in (= guest)
    
    protected override bool AllowView(Bill? Bill)
        => HasPermission("order.view") || (CurrentMember != null && Bill?.MemberCreatedId == CurrentMember.Id);

    protected override bool AllowDelete(Bill? Bill) // +Edit
        => HasPermission("admin") || (CurrentMember != null && Bill?.MemberCreatedId == CurrentMember.Id);
}