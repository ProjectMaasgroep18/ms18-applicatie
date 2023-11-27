using Microsoft.AspNetCore.Mvc;
using Ms18.Database;
using Ms18.Database.Models.TeamC.Receipt;
using Ms18.Database.Repository.TeamC.ViewModel;

namespace Ms18.Application.Controllers.TeamC;

[Route("api/v1/[controller]")]
[ApiController]
public class CostCentreController : BaseController
{
    public CostCentreController(MaasgroepContext context) : base(context) { }

    [HttpGet]
    [ActionName("costCentreGet")]
    public IActionResult CostCentreGet()
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();

        var alles = _context.CostCentre.Select(dbRec => new CostCentreViewModel(dbRec)).ToList();

        return Ok(alles);
    }

    [HttpGet("{id}")]
    [ActionName("costCentreGetById")]
    public IActionResult CostCentreGetById(int id)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();
        
        var costCentre = _context.CostCentre.Find(id);
        
        if (costCentre == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Kostenpost niet gevonden"
            });
        }
        
        return Ok(new CostCentreViewModel(costCentre));
    }

    [HttpPost]
    public IActionResult CostCentreCreate([FromBody] CostCentreViewModel costCentreViewModel)
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
        
        // Create a new cost centre from the view model
        var createdStatus = CostCentre.FromViewModel(costCentreViewModel);
        
        createdStatus.MemberCreatedId = _currentUser.Id;
        
        // Add the cost centre to the database
        _context.CostCentre.Add(createdStatus);
        _context.SaveChanges();
        
        // Return the created cost centre
        return Created($"/api/v1/CostCentre/{createdStatus.Id}", new
        {
            status = 201,
            message = "Kostenpost aangemaakt",
            costCentre = new CostCentreViewModel(createdStatus),
        });
    }

    
    [HttpPut("{id}")]
    [ActionName("costCentreUpdate")]
    public IActionResult CostCentreUpdate(long id, [FromBody] CostCentreViewModel updatedCostCentreViewModel)
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
        if (updatedCostCentreViewModel.ID != null && updatedCostCentreViewModel.ID != id)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Invalid request body, ID in URL does not match ID in request body"
            });
        }

        // Get the ID from URL (/CostCentre/{id}/) if it was not in the body
        if (updatedCostCentreViewModel.ID == null)
        {
            updatedCostCentreViewModel.ID = id;
        }

        // Retrieve the existing cost centre from your data store (e.g., database)
        CostCentre? existingStatus = _context.CostCentre.Find(id);

        // Check if the receipt with the provided ID exists
        if (existingStatus == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Kostenpost nite gevonden"
            });
        }

        if (StatusesAreEqual(existingStatus, updatedCostCentreViewModel))
        {
            // If the data is the same, return a response indicating no update was performed
            return Ok(new {
                status = 200,
                message = "Er zijn geen wijzigingen aan de kostenpost"
            });
        }

        if (updatedCostCentreViewModel.Name != null)
        {
            existingStatus.Name = updatedCostCentreViewModel.Name;
        }
        
        existingStatus.DateTimeModified = DateTime.UtcNow;

        // Save the changes to your data store (e.g., update the database record)
        _context.Update(existingStatus);
        _context.SaveChanges();

        return Ok(new CostCentreViewModel(existingStatus));
    }

    
    [HttpDelete("{id}")]
    [ActionName("costCentreDelete")]
    public IActionResult CostCentreDelete(long id)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();
        
        // Retrieve the existing cost centre from your data store (e.g., database)
        CostCentre? existingStatus = _context.CostCentre.Find(id);
        
        // Check if the cost centre with the provided ID exists
        if (existingStatus == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Kostenpost niet gevonden"
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
                message = "Kostenpost kon niet worden verwijderd" // TODO Check which dependency is causing the conflict
            });
        }
        
        return NoContent();
    }

    [HttpGet("{id}/Receipt")]
    public IActionResult CostCentreGetReceipts(long id)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();
        
        // Get receipt by ID
        CostCentre? status = _context.CostCentre.Find(id);
        
        // Check if the receipt with the provided ID exists
        if (status == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Kostenpost niet gevonden"
            });
        }
        
        // Get all receipts with this status
        var receipts = _context.Receipt
            .Where(receipt => receipt.CostCentreId == status.Id)
            .Select(receipt => new ReceiptViewModel(receipt))
            .ToList();

        receipts.ForEach(receipt => AddForeignData(receipt));
        
        return Ok(receipts);
    }
    
    
    private bool StatusesAreEqual(CostCentre existingStatus, CostCentreViewModel updatedCostCentreViewModel)
    {
        // Compare relevant properties to check if the receipt is unchanged

        // Check if given value is null or if they are equal
        bool nameEqual = (updatedCostCentreViewModel.Name == null)
                         || (existingStatus.Name == updatedCostCentreViewModel.Name);

        // Compare other properties as needed
        return nameEqual;
    }
}