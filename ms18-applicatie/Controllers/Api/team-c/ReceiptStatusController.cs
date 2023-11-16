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

    [HttpPost]
    public IActionResult ReceiptStatusCreate([FromBody] ReceiptStatusViewModel receiptStatusViewModel)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();
        
        // Validate the request body
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Invalid request body"
            });
        }
        
        // Create a new receipt status from the view model
        var createdStatus = ReceiptStatus.FromViewModel(receiptStatusViewModel);
        
        // Set the member ID of the receipt status to the ID of the current member
        var member = _context.Member.FirstOrDefault()!; // TODO Find current member
        
        createdStatus.MemberCreatedId = member.Id;
        
        // Add the receipt status to the database
        _context.ReceiptStatus.Add(createdStatus);
        _context.SaveChanges();
        
        // Return the created receipt status
        return Created($"/api/v1/ReceiptStatus/{createdStatus.Id}", new
        {
            status = 201,
            message = "Receipt status created",
            receiptStatus = new ReceiptStatusViewModel(createdStatus),
        });
    }

    
    [HttpPut("{id}")]
    [ActionName("receiptStatusUpdate")]
    public IActionResult ReceiptStatusUpdate(long id, [FromBody] ReceiptStatusViewModel updatedReceiptStatusViewModel)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();

        // Validate the request body
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Invalid request body"
            });
        }
        
        // Check if the ID in the request body matches the ID in the URL
        if (updatedReceiptStatusViewModel.ID != null && updatedReceiptStatusViewModel.ID != id)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Invalid request body, ID in URL does not match ID in request body"
            });
        }

        // Get the ID from URL (/ReceiptStatus/{id}/) if it was not in the body
        if (updatedReceiptStatusViewModel.ID == null)
        {
            updatedReceiptStatusViewModel.ID = id;
        }

        // Retrieve the existing receipt status from your data store (e.g., database)
        ReceiptStatus? existingStatus = _context.ReceiptStatus.Find(id);

        // Check if the receipt with the provided ID exists
        if (existingStatus == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Receipt status not found"
            });
        }

        if (StatusesAreEqual(existingStatus, updatedReceiptStatusViewModel))
        {
            // If the data is the same, return a response indicating no update was performed
            return Ok(new {
                status = 200,
                message = "No changes were made to the receipt status"
            });
        }

        if (updatedReceiptStatusViewModel.Name != null)
        {
            existingStatus.Name = updatedReceiptStatusViewModel.Name;
        }
        
        existingStatus.DateTimeModified = DateTime.UtcNow;

        // Save the changes to your data store (e.g., update the database record)
        _context.Update(existingStatus);
        _context.SaveChanges();

        return Ok(new ReceiptStatusViewModel(existingStatus));
    }

    
    [HttpDelete("{id}")]
    [ActionName("receiptStatusDelete")]
    public IActionResult ReceiptStatusDelete(long id)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();
        
        // Retrieve the existing receipt status from your data store (e.g., database)
        ReceiptStatus? existingStatus = _context.ReceiptStatus.Find(id);
        
        // Check if the receipt status with the provided ID exists
        if (existingStatus == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Receipt status not found"
            });
        }
        
        // Try to remove the receipt from your data store and handle if it is not possible
        try
        {
            _context.Remove(existingStatus);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            return Conflict(new
            {
                status = 409,
                message = "Could not delete receipt status" // TODO Check which dependency is causing the conflict
            });
        }
        
        return NoContent();
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
    
    
    private bool StatusesAreEqual(ReceiptStatus existingStatus, ReceiptStatusViewModel updatedReceiptStatusViewModel)
    {
        // Compare relevant properties to check if the receipt is unchanged

        // Check if given value is null or if they are equal
        bool nameEqual = (updatedReceiptStatusViewModel.Name == null)
                         || (existingStatus.Name == updatedReceiptStatusViewModel.Name);

        // Compare other properties as needed
        return nameEqual;
    }
}