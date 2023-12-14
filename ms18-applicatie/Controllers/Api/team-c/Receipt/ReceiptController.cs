using Maasgroep.Database.Receipts;
using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.Services;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class ReceiptController : ControllerBase
{
    private readonly IReceiptRepository<Receipt, ReceiptHistory> _receipt;
    private readonly IMemberService _memberService;

    public ReceiptController(IReceiptRepository<Receipt, ReceiptHistory> receipt, IMemberService memberService) 
    { 
        _receipt = receipt; 
        _memberService = memberService;
    }

    [HttpGet]
    [ActionName("receiptGet")]
    public IActionResult ReceiptGet([FromQuery] int offset = default, [FromQuery] int limit = default, [FromQuery] bool includeDeleted = default)
    {
        if (!MemberExists(1)) // Toegangscontrole
            return Forbidden();

        var result = _receipt.ListAll(offset, limit, includeDeleted);

		if (result == null)
			return NotFound(new
			{
				status = 404,
				message = "Declaratie niet gevonden"
			});

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ActionName("receiptGetById")]
    public IActionResult ReceiptGetById(int id)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

		var result = _receipt.GetById(id);

		if (result == null)
			return NotFound(new
			{
				status = 404,
				message = "Declaratie niet gevonden"
			});

		return Ok(result);
    }

    [HttpPost]
    [ActionName("receiptCreate")]
    public IActionResult ReceiptCreate([FromBody] ReceiptModel receiptModel)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

		// Validate the request body
		if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Ongeldige data opgegeven"
            });
        }

		var receipt = _receipt.Create(receiptModel, 1);

        if (receipt == null) {
            return BadRequest(new
            {
                status = 400,
                message = "Ongeldige data opgegeven"
            }); 
        }

        return Ok(receipt);
    }

    [HttpPut]
    [ActionName("receiptUpdate")]
    public IActionResult ReceiptUpdate([FromBody] ReceiptModel receiptModel)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

		// Validate the request body
		if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Ongeldige data opgegeven"
            });
        }

        var receiptToUpdate = new ReceiptModelUpdateDb()
        {
            ReceiptModel = receiptModel,
            Member = new Maasgroep.SharedKernel.ViewModels.Admin.MemberModel() { Id = _memberService.GetMember(1).Id }
        };

		var result = _receiptRepository.Modify(receiptToUpdate);

        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    [ActionName("receiptDelete")]
    public IActionResult ReceiptDelete(long id)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

		// Retrieve the existing receipt from your data store (e.g., database)
		ReceiptModel? existingReceipt = _receiptRepository.GetReceipt(id);
        
        // Check if the receipt with the provided ID exists
        if (existingReceipt == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Declaratie niet gevonden"
            });
        }

        var receiptToDelete = new ReceiptModelDeleteDb() { Member = _memberService.GetMember(1), Receipt = existingReceipt };
        
        // Try to remove the receipt from your data store and handle if it is not possible
        try
        {
            _receiptRepository.Delete(receiptToDelete); //TODO: deze logica moet nog gebouwd
        }
        catch (Exception)
        {
            return Conflict(new
            {
                status = 409,
                message = "Declaratie kon niet worden verwijderd" // TODO Check which dependency is causing the conflict
            });
        }
        
        return NoContent();
    }

    [HttpPost("{id}/Photo")]
    [ActionName("receiptPhotosAdd")]
    public IActionResult ReceiptAddPhoto(long id, [FromBody] PhotoModelCreate photo)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

		// Validate the request body
		if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Ongeldige data opgegeven"
            });
        }

		// Get the receipt by the ID
		ReceiptModel? existingReceipt = _receiptRepository.GetReceipt(id);

		// Check if the receipt with the provided ID exists
		if (existingReceipt == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Declaratie niet gevonden"
            });
        }

        // Create a new photo from the view model
        var createdPhoto = new PhotoModelCreateDb()
        { 
            PhotoModel = photo,
            ReceiptId = id,
            Member = _memberService.GetMember(1)
        };

        // Add the photo to the database
        var createPhotoId = _receiptRepository.Add(createdPhoto);
        if (createPhotoId == null) {
            return BadRequest(new
            {
                status = 400,
                message = "Foto kon niet worden aangemaakt"
            }); 
        }
        var photoToReturn = _receiptRepository.GetPhoto(createPhotoId ?? 0);
        
        // Return the created photo
        return Created($"/api/v1/receipt/{id}/Photo/{createPhotoId}", new
        {
            status = 201,
            message = "Foto aangemaakt",
            photo = photoToReturn
		});
    }

    [HttpGet("{id}/Photo")]
    [ActionName("receiptPhotosGet")]
    public IActionResult ReceiptGetPhotos(long id, [FromQuery] int offset = 0, [FromQuery] int limit = 100, [FromQuery] bool includeDeleted = false)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

		// Get the receipt by the ID
		ReceiptModel? existingReceipt = _receiptRepository.GetReceipt(id);

		// Check if the receipt with the provided ID exists
		if (existingReceipt == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Declaratie niet gevonden"
            });
        }

        // Get all photos for the receipt
        var photos = _receiptRepository.GetPhotosByReceipt(id, offset, limit, includeDeleted);

        return Ok(photos);
    }

    [HttpPost("{id}/Approve")]
    [ActionName("approveReceipt")]
    public IActionResult ApproveReceipt(long id, [FromBody] ReceiptApprovalModelCreate approvalModel)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

		// Validate the request body
		if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Ongeldige data opgegeven"
            });
        }

		// Get the receipt by the ID
		ReceiptModel? existingReceipt = _receiptRepository.GetReceipt(id);

		// Check if the receipt with the provided ID exists
		if (existingReceipt == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Declaratie niet gevonden"
            });
        }

        // Create a new approval record from the view model
        var approval = new ReceiptApprovalModelCreateDb()
        { 
            Approval = approvalModel,
            Member = _memberService.GetMember(1)
		};

        // Add the photo to the database
        _receiptRepository.AddApproval(approval);
        
        // Return the created photo
        return Created($"/api/v1/receipt/{id}", new
        {
            status = 201,
            message = "Declaratie goedgekeurd",
            approval = approvalModel,
        });
    }

    private bool MemberExists(long id) => _memberService.MemberExists(id);
    private IActionResult Forbidden() => Forbid("Je hebt geen toegang tot deze functie");
    
}