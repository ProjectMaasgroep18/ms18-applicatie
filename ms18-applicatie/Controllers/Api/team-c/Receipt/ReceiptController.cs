using Maasgroep.Database.Receipts;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

public class ReceiptController : EditableRepositoryController<IReceiptRepository, Receipt, ReceiptModel, ReceiptHistory>
{
	protected readonly IReceiptPhotoRepository Photos;
	protected readonly IReceiptApprovalRepository Approvals;

    public ReceiptController(IReceiptRepository repository, IReceiptPhotoRepository photos, IReceiptApprovalRepository approvals) : base(repository)
    {
        Photos = photos;
        Approvals = approvals;
    }

    [HttpPost("{id}/Photo")]
    public IActionResult ReceiptAddPhoto(long id, [FromBody] ReceiptPhotoModel model)
    {
        model.ReceiptId = id;
        var photo = Photos.Create(model, 1);
        if (photo == null)
            return BadRequest(photo);
        return Created($"/api/v1/ReceiptPhotos/{photo.Id}", photo);
    }

    [HttpGet("{id}/Photo")]
    public IActionResult ReceiptGetPhotos(long id, [FromQuery] int offset = default, [FromQuery] int limit = default, [FromQuery] bool includeDeleted = default)
        => Ok(Photos.ListByReceipt(id, offset, limit, includeDeleted));

    [HttpPost("{id}/Approve")]
    public IActionResult ReceiptApprove(long id, [FromBody] ReceiptApprovalModel model)
    {
        model.ReceiptId = id;
        var approval = Approvals.Create(model, 1);
        if (approval == null)
            return BadRequest(approval);
        return Ok(approval);
    }
}