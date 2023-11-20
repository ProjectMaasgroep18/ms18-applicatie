using Maasgroep.Database;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Repository.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ms18_applicatie.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class ReceiptStatusController : BaseController
{
    public ReceiptStatusController(MaasgroepContext context) : base(context) { }

    [HttpGet]
    [ActionName("receiptStatusGet")]
    public IActionResult ReceiptStatusGet()
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();

        var alles = _context.ReceiptStatus.Select(dbRec => new ReceiptStatusViewModel(dbRec)).ToList();

        return Ok(alles);
    }

    [HttpGet("{id}")]
    [ActionName("receiptStatusGetById")]
    public IActionResult ReceiptStatusGetById(int id)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();
        
        var receiptStatus = _context.ReceiptStatus.Find(id);
        
        if (receiptStatus == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Status not found"
            });
        }
        
        return Ok(new ReceiptStatusViewModel(receiptStatus));
    }

    [HttpGet("{id}/Receipt")]
    public IActionResult ReceiptStatusGetReceipts(long id)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();
        
        // Get receipt by ID
        ReceiptStatus? status = _context.ReceiptStatus.Find(id);
        
        // Check if the receipt with the provided ID exists
        if (status == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Receipt status not found"
            });
        }
        
        // Get all receipts with this status
        var receipts = _context.Receipt
            .Where(receipt => receipt.ReceiptStatusId == status.Id)
            .Select(receipt => new ReceiptViewModel(receipt))
            .ToList();

        receipts.ForEach(receipt => AddForeignData(receipt));
        
        return Ok(receipts);
    }
}