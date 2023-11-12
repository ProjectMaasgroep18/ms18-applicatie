using Maasgroep.Database;
using Maasgroep.Database.Members;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.team_c.Repository.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ms18_applicatie.Controllers.Api.team_c;

[Route("api/v1/[controller]")]
[ApiController]
public class ReceiptController : ControllerBase
{
    private readonly MaasgroepContext _context;

    public ReceiptController(MaasgroepContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ActionName("receiptGet")]
    public IActionResult ReceiptGet()
    {
        return Ok(_context.Receipt.Select(dbRec => new ReceiptViewModel(dbRec)).ToList());
    }

    [HttpGet("{id}")]
    [ActionName("receiptGetById")]
    public IActionResult ReceiptGetById(int id)
    {
        ReceiptViewModel? receiptViewModel = _context.Receipt
            .Where(dbRec => dbRec.Id == id)
            .Select(dbRec => new ReceiptViewModel(dbRec))
            .FirstOrDefault();

        if (receiptViewModel == null)
            return NotFound(new
            {
                status = 404,
                message = "Receipt not found"
            });

        return Ok(receiptViewModel);
    }

    [HttpPost]
    [ActionName("receiptCreate")]
    public IActionResult ReceiptCreate([FromBody] ReceiptViewModel receiptViewModel)
    {
        // Validate the request body
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Invalid request body"
            });
        }

        var createdReceipt = Receipt.FromViewModel(receiptViewModel);

        var member = _context.Member.FirstOrDefault()!; // TODO Find current member
        createdReceipt.MemberCreatedId = member.Id;

        var status = _context.ReceiptStatus.FirstOrDefault()!; // TODO Find correct status
        createdReceipt.ReceiptStatusId = status.Id;

        var costCentre = _context.CostCentre.FirstOrDefault()!; // TODO Let user select CostCentre
        createdReceipt.CostCentreId = costCentre.Id;

        try
        {
            _context.Receipt.Add(createdReceipt);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            return UnprocessableEntity(new
            {
                status = 422,
                message = "Could not create receipt",
            });
        }

        return Created($"/api/v1/receipt/{createdReceipt.Id}", new
        {
            status = 201,
            message = "Receipt created",
            receipt = new ReceiptViewModel(createdReceipt)
        });
    }

    [HttpPut("{id}")]
    [ActionName("receiptUpdate")]
    public IActionResult ReceiptUpdate(long id, [FromBody] ReceiptViewModel updatedReceiptViewModel)
    {
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
        if (updatedReceiptViewModel.ID != id)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Invalid request body, ID in URL does not match ID in request body"
            });
        }

        if (updatedReceiptViewModel.ID == null)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Invalid request body, missing ID"
            });
        }

        // Retrieve the existing receipt from your data store (e.g., database)
        Receipt? existingReceipt = _context.Receipt.Find(id);

        // Check if the receipt with the provided ID exists
        if (existingReceipt == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Receipt not found"
            });
        }

        if (ReceiptsAreEqual(existingReceipt, updatedReceiptViewModel))
        {
            // If the data is the same, return a response indicating no update was performed
            return Ok(new {
                status = 200,
                message = "No changes were made to the receipt"
            });
        }

        if (updatedReceiptViewModel.Amount != null)
        {
            existingReceipt.Amount = updatedReceiptViewModel.Amount;
        }

        if (updatedReceiptViewModel.Note != null)
        {
            existingReceipt.Note = updatedReceiptViewModel.Note;
        }
        
        existingReceipt.DateTimeModified = DateTime.UtcNow;

        // Save the changes to your data store (e.g., update the database record)
        _context.Update(existingReceipt);
        _context.SaveChanges();

        return Ok(new ReceiptViewModel(existingReceipt));
    }
    
    [HttpDelete("{id}")]
    [ActionName("receiptDelete")]
    public IActionResult ReceiptRemove(long id)
    {
        
        // Retrieve the existing receipt from your data store (e.g., database)
        Receipt? existingReceipt = _context.Receipt.Find(id);
        
        // Check if the receipt with the provided ID exists
        if (existingReceipt == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Receipt not found"
            });
        }
        
        // Try to remove the receipt from your data store and handle if it is not possible
        try
        {
            _context.Remove(existingReceipt);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            return Conflict(new
            {
                status = 409,
                message = "Could not delete receipt" // TODO Check which dependency is causing the conflict
            });
        }
        
        return NoContent();
    }
    
    private bool ReceiptsAreEqual(Receipt existingReceipt, ReceiptViewModel updatedReceiptViewModel)
    {
        // Compare relevant properties to check if the receipt is unchanged

        // Check if given value is null or if they are equal
        bool amountEqual = (updatedReceiptViewModel.Amount == null)
                           || (existingReceipt.Amount == updatedReceiptViewModel.Amount);

        // Check if given value is null or if they are equal
        bool noteEqual = (updatedReceiptViewModel.Note == null)
                         || (existingReceipt.Note == updatedReceiptViewModel.Note);

        // Compare other properties as needed
        return amountEqual
               && noteEqual;
    }
    
}