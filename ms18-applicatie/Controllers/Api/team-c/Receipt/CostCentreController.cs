using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Services;

namespace ms18_applicatie.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class CostCentreController : ControllerBase
{
	private readonly IReceiptRepository _receiptRepository;
	private readonly IMemberService _memberService;

	public CostCentreController(IReceiptRepository receiptRepository, IMemberService memberService) 
    {
		_receiptRepository = receiptRepository;
		_memberService = memberService;
	}

    [HttpGet]
    [ActionName("costCentreGet")]
    public IActionResult CostCentreGet()
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

        return Ok(_receiptRepository.GetCostCentres(0, int.MaxValue));
    }

    [HttpGet("{id}")]
    [ActionName("costCentreGetById")]
    public IActionResult CostCentreGetById(int id)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

        var costCentre = _receiptRepository.GetCostCentre(id);
        
        if (costCentre == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Kostenpost niet gevonden"
            });
        }
        
        return Ok(costCentre);
    }

    [HttpPost]
    public IActionResult CostCentreCreate([FromBody] CostCentreModelCreate costCentreModel)
    {
		if (!MemberExists(1)) // Toegangscontrole
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
		var costCentreToAdd = new CostCentreModelCreateDb()
		{
			CostCentre = costCentreModel,
			Member = _memberService.GetMember(1)
		};

        // Add the cost centre to the database
        var costCentreAdded = _receiptRepository.Add(costCentreToAdd);
        
        // Return the created cost centre
        return Created($"/api/v1/CostCentre/{costCentreAdded}", new
        {
            status = 201,
            message = "Kostenpost aangemaakt",
            costCentre = costCentreAdded,
        });
    }

    [HttpPut("{id}")]
    [ActionName("costCentreUpdate")]
    public IActionResult CostCentreUpdate(long id, [FromBody] CostCentreModel updatedCostCentreViewModel)
    {
		if (!MemberExists(1)) // Toegangscontrole
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

        // Retrieve the existing cost centre from your data store (e.g., database)
        CostCentreModel? existingCostCentre = _receiptRepository.GetCostCentre(id);

        // Check if the receipt with the provided ID exists
        if (existingCostCentre == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Kostenpost niet gevonden"
            });
        }

        if (existingCostCentre.Equals(updatedCostCentreViewModel))
        {
            // If the data is the same, return a response indicating no update was performed
            return Ok(new {
                status = 200,
                message = "Er zijn geen wijzigingen aan de kostenpost"
            });
        }

        var costCentreToUpdate = new CostCentreModelUpdateDb()
        { 
            CostCentre = updatedCostCentreViewModel, 
            Member = _memberService.GetMember(1)
		};
        
        // Save the changes to your data store (e.g., update the database record)
        var result = _receiptRepository.Modify(costCentreToUpdate);

        return Ok(result);
    }
   
    [HttpDelete("{id}")]
    [ActionName("costCentreDelete")]
    public IActionResult CostCentreDelete(long id)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

        // Retrieve the existing cost centre from your data store (e.g., database)
        var existingCostCentre = _receiptRepository.GetCostCentre(id);
        
        // Check if the cost centre with the provided ID exists
        if (existingCostCentre == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Kostenpost niet gevonden"
            });
        }

        var costCentreToDelete = new CostCentreModelDeleteDb()
        {
            CostCentre = existingCostCentre,
            Member = _memberService.GetMember(1)
        };
        
        // Try to remove the receipt from your data store and handle if it is not possible
        try
        {
            _receiptRepository.Delete(costCentreToDelete);
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
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

        // Get receipt by ID
        var costCentreExists = _receiptRepository.GetCostCentre(id);
        
        // Check if the receipt with the provided ID exists
        if (costCentreExists == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Kostenpost niet gevonden"
            });
        }

        // Get all receipts with this status
        var costCentres = _receiptRepository.GetReceiptsByCostCentre(id, 0, int.MaxValue);
        
        return Ok(costCentres);
    }
    
	private bool MemberExists(long id) => _memberService.MemberExists(id);
	private IActionResult Forbidden() => Forbid("Je hebt geen toegang tot deze functie");
}