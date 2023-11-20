using Maasgroep.Database;
using Maasgroep.Database.Photos;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Repository.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ms18_applicatie.Controllers.Api;

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


        var costCentres = _context.CostCentre
            .OrderBy(dbRec => dbRec.Id)
            .Select(dbRec => new CostCentreViewModel(dbRec))
            .ToList();
        
        return Ok(costCentres);
    }

    [HttpGet("{id}")]
    [ActionName("costCentreGetById")]
    public IActionResult CostCentreGetById(int id)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();

        var costCentre = _context.CostCentre
            .Where(cc => cc.Id == id)
            .Select(dbRec => new CostCentreViewModel(dbRec))
            .FirstOrDefault();

        if (costCentre == null)
            return NotFound(new
            {
                status = 404,
                message = "CostCentre not found"
            });
        
        return Ok(costCentre);
    }

    [HttpPost]
    [ActionName("costCentreCreate")]
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

        var createdCostCentre = new CostCentre(costCentreViewModel);

        createdCostCentre.MemberCreatedId = _currentUser.Id;
        
        try
        {
            _context.CostCentre.Add(createdCostCentre);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            return UnprocessableEntity(new
            {
                status = 422,
                message = "Could not create costCentre"
            });
        }


        return Created($"/api/v1/CostCentre/{createdCostCentre.Id}", new
        {
            status = 201,
            message = "CostCentre created",
            costCentre = new CostCentreViewModel(createdCostCentre)
        });
    }
    
    [HttpPut("{id}")]
    [ActionName("costCentreUpdate")]
    public IActionResult CostCentreUpdate(long id, [FromBody] CostCentreViewModel updatedCostCentreModel)
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
        if (updatedCostCentreModel.Id != id)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Invalid request body, ID in URL does not match ID in request body"
            });
        }

        // Get the ID from URL (/Receipt/{id}/) if it was not in the body
        if (updatedCostCentreModel.Id == null)
        {
            updatedCostCentreModel.Id = id;
        }

        // Retrieve the existing receipt from your data store (e.g., database)
        CostCentre? existingCostCentre = _context.CostCentre.Find(id);

        // Check if the receipt with the provided ID exists
        if (existingCostCentre == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "CostCentre not found"
            });
        }

        if (CostCentresAreEqual(existingCostCentre, updatedCostCentreModel))
        {
            // If the data is the same, return a response indicating no update was performed
            return Ok(new {
                status = 200,
                message = "No changes were made to the CostCentre"
            });
        }

        if (updatedCostCentreModel.Name != null)
        {
            existingCostCentre.Name = updatedCostCentreModel.Name;
        }
        
        existingCostCentre.DateTimeModified = DateTime.UtcNow;

        // Save the changes to your data store (e.g., update the database record)
        _context.Update(existingCostCentre);
        _context.SaveChanges();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [ActionName("costCentreDelete")]
    public IActionResult ReceiptDelete(long id)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();
        
        // Retrieve the existing receipt from your data store (e.g., database)
        CostCentre? existingCostCentre = _context.CostCentre.Find(id);
        
        // Check if the receipt with the provided ID exists
        if (existingCostCentre == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "CostCentre not found"
            });
        }
        
        // Try to remove the receipt from your data store and handle if it is not possible
        try
        {
            _context.Remove(existingCostCentre);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            return Conflict(new
            {
                status = 409,
                message = "Kostenplaats kon niet worden verwijderd" // TODO Check which dependency is causing the conflict
            });
        }
        
        return NoContent();
    }
    
    private static bool CostCentresAreEqual(CostCentre existingCC, CostCentreViewModel updatedCCViewModel)
    {
        // Compare relevant properties to check if the receipt is unchanged

        // Check if given value is null or if they are equal
        bool nameEqual = (existingCC.Name == updatedCCViewModel.Name);

        // Compare other properties as needed
        return nameEqual;
    }

    
}