using Maasgroep.Exceptions;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.DataModels.Receipts;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public class ReceiptController : EditableRepositoryController<IReceiptRepository, Receipt, ReceiptModel, ReceiptData, ReceiptHistory>
{
    protected readonly IReceiptPhotoRepository Photos;
    protected readonly IReceiptApprovalRepository Approvals;
    public override string ItemName { get => "Declaratie"; }

    public ReceiptController(IReceiptRepository repository, IReceiptPhotoRepository photos, IReceiptApprovalRepository approvals) : base(repository)
    {
        Photos = photos;
        Approvals = approvals;
    }

    protected override bool AllowList()
        => HasPermission("receipt.approve");

    protected override bool AllowView(Receipt? receipt)
        => HasPermission("receipt.approve") || HasPermission("receipt.pay") || (CurrentMember != null && receipt?.MemberCreatedId == CurrentMember.Id);

    protected override bool AllowCreate(ReceiptData receipt)
        => HasPermission("receipt");

    protected override bool AllowDelete(Receipt? receipt) // +Edit
        => HasPermission("admin") || (CurrentMember != null && receipt?.MemberCreatedId == CurrentMember.Id);

    [HttpPost("{id}/Photo")]
    public IActionResult ReceiptAddPhoto(long id, [FromBody] ReceiptPhotoData data)
    {
        var receipt = Repository.GetById(id);
        if (receipt == null || !AllowView(receipt))
            NotFound();
        if (!AllowDelete(receipt!))
            NoAccess();
        data.ReceiptId = id;
        var photo = Photos.Create(data, CurrentMember?.Id);
        if (photo == null)
            NotWritable();
        return Created($"/api/v1/ReceiptPhotos/{photo!.Id}", photo);
    }

    [HttpGet("{id}/Photo")]
    public IActionResult ReceiptGetPhotos(long id, [FromQuery] int offset = default, [FromQuery] int limit = default, [FromQuery] bool includeDeleted = default)
    {
        var receipt = Repository.GetById(id);
        if (receipt == null || !AllowView(receipt))
            NotFound();
        return Ok(Photos.ListByReceipt(id, offset, limit, includeDeleted));
    }

    [HttpGet("{id}/Approve")]
    public IActionResult ReceiptGetApprovals(long id, [FromQuery] int offset = default, [FromQuery] int limit = default)
        => Ok(Approvals.ListByReceipt(id, offset, limit));

    [HttpPost("{id}/Approve")]
    public IActionResult ReceiptApprove(long id, [FromBody] ReceiptApprovalData data)
    {
        var receipt = Repository.GetById(id);
        if (receipt == null || !AllowView(receipt))
            NotFound();
        data.ReceiptId = id;
        var approval = Approvals.Create(data, CurrentMember?.Id);
        if (approval == null)
            NotWritable();
        return Ok(Approvals.GetModel(approval!));
    }

    [HttpGet("Payable")]
    public IActionResult GetPayableReceipts([FromQuery] int offset = default, [FromQuery] int limit = default, [FromQuery] bool includeDeleted = default)
    {
        if (!HasPermission("receipt.pay"))
            NoAccess();
        return Ok(Repository.ListPayable(offset, limit, includeDeleted));
    }
}