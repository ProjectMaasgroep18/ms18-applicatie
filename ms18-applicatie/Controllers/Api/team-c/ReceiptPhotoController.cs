using Maasgroep.Database;
using Maasgroep.Database.Members;
using Maasgroep.Database.Photos;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Repository.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ms18_applicatie.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class ReceiptPhotoController : ControllerBase
{
    private readonly MaasgroepContext _context;

    public ReceiptPhotoController(MaasgroepContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public IActionResult PhotoGet(long id)
    {
        
        var photo = _context.Photo.Find(id);
        
        if (photo == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Photo not found"
            });
        }
        
        return Ok(PhotoViewModel.FromDatabaseModel(photo));
    }

    [HttpPost]
    public IActionResult PhotoCreate([FromBody] PhotoViewModel photoViewModel)
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
        
        // Create a new photo from the view model
        var createdPhoto = Photo.FromViewModel(photoViewModel);
        
        // Set the member ID of the photo to the ID of the current member
        var member = _context.Member.FirstOrDefault()!; // TODO Find current member
        
        createdPhoto.MemberCreatedId = member.Id;
        
        // Add the photo to the database
        _context.Photo.Add(createdPhoto);
        _context.SaveChanges();
        
        // Return the created photo
        return Created($"/api/v1/ReceiptPhoto/{createdPhoto.Id}", new
        {
            status = 201,
            message = "Photo created",
            photo = PhotoViewModel.FromDatabaseModel(createdPhoto)
        });
    }

    
    [HttpDelete("{id}")]
    [ActionName("photoDelete")]
    public IActionResult photoRemove(long id)
    {
        
        // Retrieve the existing receipt from your data store (e.g., database)
        Photo? existingPhoto = _context.Photo.Find(id);
        
        // Check if the receipt with the provided ID exists
        if (existingPhoto == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Photo not found"
            });
        }
        
        // Try to remove the receipt from your data store and handle if it is not possible
        try
        {
            _context.Remove(existingPhoto);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            return Conflict(new
            {
                status = 409,
                message = "Could not delete photo" // TODO Check which dependency is causing the conflict
            });
        }
        
        return NoContent();
    }

}