using Maasgroep.SharedKernel.Interfaces.Receipts;
using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Services;

namespace ms18_applicatie.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class ReceiptPhotoController : ControllerBase
{
	private readonly IReceiptRepository _receiptRepository;
	private readonly IMemberService _memberService;

	public ReceiptPhotoController(IReceiptRepository receiptRepository, IMemberService memberService)
    {
		_receiptRepository = receiptRepository;
		_memberService = memberService;
	}

    [HttpGet("{id}")]
    public IActionResult PhotoGet(long id)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

        var photo = _receiptRepository.GetPhoto(id);
        
        if (photo == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Photo not found"
            });
        }
        
        return Ok(photo);
    }
    
    [HttpDelete("{id}")]
    [ActionName("photoDelete")]
    public IActionResult PhotoDelete(long id)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

        // Retrieve the existing receipt from your data store (e.g., database)
        var existingPhoto = _receiptRepository.GetPhoto(id);
        
        // Check if the receipt with the provided ID exists
        if (existingPhoto == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Photo not found"
            });
        }
        
        // Try to remove the photo from your data store and handle if it is not possible
        try
        {
            _receiptRepository.DeletePhoto(id);
        }
        catch (Exception)
        {
            return Conflict(new
            {
                status = 409,
                message = "Foto kon niet worden verwijderd" // TODO Check which dependency is causing the conflict
            });
        }
        
        return NoContent();
    }
    
	private bool MemberExists(long id) => _memberService.MemberExists(id);
	private IActionResult Forbidden() => Forbid("Je hebt geen toegang tot deze functie");
}