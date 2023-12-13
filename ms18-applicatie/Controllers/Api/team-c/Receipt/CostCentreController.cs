using Maasgroep.Database.Receipts;
using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.Services;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class CostCentreController : ControllerBase
{
	private readonly ICostCentreRepository<CostCentre, CostCentreHistory> _costCentre;
	private readonly IReceiptRepository<Receipt, ReceiptHistory> _receipt;
	private readonly IMemberService _memberService;

	public CostCentreController(ICostCentreRepository<CostCentre, CostCentreHistory> costCentre, IReceiptRepository<Receipt, ReceiptHistory> receipt, IMemberService member) 
    {
		_costCentre = costCentre;
		_receipt = receipt;
		_memberService = member;
	}

    [HttpGet]
    [ActionName("costCentreGet")]
    public IActionResult CostCentreGet([FromQuery] int offset = default, [FromQuery] int limit = default, [FromQuery] bool includeDeleted = default)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

        return Ok(_costCentre.ListAll(offset, limit, includeDeleted));
    }

    [HttpGet("{id}")]
    [ActionName("costCentreGetById")]
    public IActionResult CostCentreGetById(long id)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

        var costCentre = _costCentre.GetById(id);
        
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
    public IActionResult CostCentreCreate([FromBody] CostCentreModel costCentreModel)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

		// Validate the request body
		if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Ongeldige gegevens"
            });
        }

        var costCentre = _costCentre.Create(costCentreModel, 1);

        if (costCentre == null)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Er is een onbekende fout opgetreden"
            });
        }

        // Return the created cost centre
        return Created($"/api/v1/CostCentre/{costCentre.Id}", new
        {
            status = 201,
            message = "Kostenpost aangemaakt",
            costCentre,
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
                message = "Ongeldige gegevens"
            });
        }

        var costCentre = _costCentre.Update(id, updatedCostCentreViewModel, 1);

        if (costCentre == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Kostenpost niet gevonden"
            });
        }

        // Return the created cost centre
        return Ok(new
        {
            status = 200,
            message = "Kostenpost geüpdate",
            costCentre,
        });
    }
   
    [HttpDelete("{id}")]
    [ActionName("costCentreDelete")]
    public IActionResult CostCentreDelete(long id)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

        var deleted = _costCentre.Delete(id, 1);

        if (!deleted)
        {
            return NotFound(new
            {
                status = 404,
                message = "Kostenpost niet gevonden"
            });
        }
       
        return NoContent();
    }

    [HttpGet("{id}/Receipt")]
    public IActionResult CostCentreGetReceipts(long id, [FromQuery] int offset = default, [FromQuery] int limit = default, [FromQuery] bool includeDeleted = default)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();
        
        // Check if the receipt with the provided ID exists
        if (!_costCentre.Exists(id))
        {
            return NotFound(new
            {
                status = 404,
                message = "Kostenpost niet gevonden"
            });
        }

        // Get receipts by cost centre ID
        var receipts = _receipt.ListByCostCentre(id, offset, limit, includeDeleted);
        
        return Ok(receipts);
    }
    
	private bool MemberExists(long id) => _memberService.MemberExists(id);
	private IActionResult Forbidden() => Forbid("Je hebt geen toegang tot deze functie");
}