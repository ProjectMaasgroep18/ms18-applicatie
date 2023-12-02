using Maasgroep.Database;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Repository.ViewModel;
using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Microsoft.AspNetCore.Mvc;

namespace ms18_applicatie.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class ReceiptController : BaseController
{
    private readonly IReceiptRepository _receiptRepository;

    public ReceiptController(MaasgroepContext context, IReceiptRepository receiptRepository) : base(context) 
    { 
        _receiptRepository = receiptRepository; 
    }

    [HttpGet]
    [ActionName("receiptGet")]
    public IActionResult ReceiptGet()
    {
        if (_currentUser == null) // Toegangscontrole
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
        if (_currentUser == null) // Toegangscontrole
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

        var receiptToAdd = new ReceiptModelCreateDb()
        {
            ReceiptModel = receiptModelCreate
        ,   Member = new Maasgroep.SharedKernel.ViewModels.Admin.Member() { Id = _currentUser.Id }
        };

		var result = _receiptRepository.AddReceipt(receiptToAdd);

        return Ok(result);
    }

    [HttpPut]
    [ActionName("receiptUpdate")]
    public IActionResult ReceiptUpdate([FromBody] ReceiptModel receiptModel)
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

        var receiptToUpdate = new ReceiptModelUpdateDb()
        {
            ReceiptModel = receiptModel,
            Member = new Maasgroep.SharedKernel.ViewModels.Admin.Member() { Id = _currentUser.Id }
        };

		var result = _receiptRepository.ModifyReceipt(receiptToUpdate);

        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    [ActionName("receiptDelete")]
    public IActionResult ReceiptDelete(long id)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();
        
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
    public IActionResult ReceiptAddPhoto(long id, [FromBody] PhotoViewModel photoViewModel)
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
        
        // Get the receipt by the ID
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
        
        // Create a new photo from the view model
        var createdPhoto = Photo.FromViewModel(photoViewModel);
        
        // Set the receipt ID of the photo to the ID of the receipt
        createdPhoto.ReceiptId = existingReceipt.Id;
        
        // Set the member created ID of the photo to the ID of the current user
        createdPhoto.MemberCreatedId = _currentUser.Id;
        
        // Add the photo to the database
        _context.Photo.Add(createdPhoto);
        _context.SaveChanges();
        
        // Return the created photo
        return Created($"/api/v1/receipt/{id}/Photo/{createdPhoto.Id}", new
        {
            status = 201,
            message = "Photo created",
            photo = new PhotoViewModel(createdPhoto)
        });
    }

    [HttpGet("{id}/Photo")]
    [ActionName("receiptPhotosGet")]
    public IActionResult ReceiptGetPhotos(long id)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();
        
        // Get the receipt by the ID
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
        
        // Get all photos for the receipt
        var photos = _context.Photo
            .Where(photo => photo.ReceiptId == existingReceipt.Id)
            .Select(photo => new PhotoViewModel(photo))
            .ToList();
        
        return Ok(photos);
    }

    [HttpPost("{id}/Approve")]
    [ActionName("approveReceipt")]
    public IActionResult ApproveReceipt(long id, [FromBody] ReceiptApprovalViewModel approvalViewModel)
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
        
        // Get the receipt by the ID
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

        // Create a new approval record from the view model
        var approval = ReceiptApproval.FromViewModel(approvalViewModel);
        
        // Set the receipt ID of the approval to the ID of the receipt
        approval.ReceiptId = existingReceipt.Id;
        
        // Set the member created ID of the photo to the ID of the current user
        approval.MemberCreatedId = _currentUser.Id;
        
        // Add the photo to the database
        _context.ReceiptApproval.Add(approval);
        _context.SaveChanges();
        
        // Return the created photo
        return Created($"/api/v1/receipt/{id}", new
        {
            status = 201,
            message = "Receipt approved",
            approval = new ReceiptApprovalViewModel(approval),
        });
    }
    
    private static bool ReceiptsAreEqual(Receipt existingReceipt, ReceiptViewModel updatedReceiptViewModel)
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