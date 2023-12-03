using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Services;

namespace ms18_applicatie.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class ReceiptController : ControllerBase
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IMemberService _memberService;

    public ReceiptController(IReceiptRepository receiptRepository, IMemberService memberService) 
    { 
        _receiptRepository = receiptRepository; 
        _memberService = memberService;
    }

    [HttpGet]
    [ActionName("receiptGet")]
    public IActionResult ReceiptGet()
    {
        if (!MemberExists(1)) // Toegangscontrole
            return Forbidden();

        var result = _receiptRepository.GetReceipts(0, int.MaxValue);

		if (result == null)
			return NotFound(new
			{
				status = 404,
				message = "Receipt not found"
			});

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ActionName("receiptGetById")]
    public IActionResult ReceiptGetById(int id)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

		var result = _receiptRepository.GetReceipt(id);

		if (result == null)
			return NotFound(new
			{
				status = 404,
				message = "Receipt not found"
			});

		return Ok(result);
    }

    [HttpPost]
    [ActionName("receiptCreate")]
    public IActionResult ReceiptCreate([FromBody] ReceiptModelCreate receiptModelCreate)
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

        var receiptToAdd = new ReceiptModelCreateDb()
        {
            ReceiptModel = receiptModelCreate
        ,   Member = _memberService.GetMember(1)
        };

		var result = _receiptRepository.Add(receiptToAdd);

        return Ok(result);
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
                message = "Invalid request body"
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
                message = "Receipt not found"
            });
        }
        
        // Try to remove the receipt from your data store and handle if it is not possible
        try
        {
            _receiptRepository.DeleteReceipt(existingReceipt); //TODO: deze logica moet nog gebouwd
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
                message = "Invalid request body"
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
                message = "Receipt not found"
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
        var photoToReturn = _receiptRepository.GetPhoto(createPhotoId);
        
        // Return the created photo
        return Created($"/api/v1/receipt/{id}/Photo/{createPhotoId}", new
        {
            status = 201,
            message = "Photo created",
            photo = photoToReturn
		});
    }

    [HttpGet("{id}/Photo")]
    [ActionName("receiptPhotosGet")]
    public IActionResult ReceiptGetPhotos(long id)
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
                message = "Receipt not found"
            });
        }

        // Get all photos for the receipt
        var photos = _receiptRepository.GetPhotosByReceipt(id, 0, int.MaxValue);

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
                message = "Invalid request body"
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
                message = "Receipt not found"
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
            message = "Receipt approved",
            approval = approvalModel,
        });
    }

    private bool MemberExists(long id) => _memberService.MemberExists(id);
    private IActionResult Forbidden() => Forbid("Je hebt geen toegang tot deze functie");
    
}