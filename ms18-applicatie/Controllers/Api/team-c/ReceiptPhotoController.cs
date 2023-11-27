using Maasgroep.Database;
using Maasgroep.Database.Members;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Repository.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ms18_applicatie.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class ReceiptPhotoController : BaseController
{
    public ReceiptPhotoController(MaasgroepContext context) : base(context) { }

    [HttpGet("{id}")]
    public IActionResult PhotoGet(long id)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();
        
        var photo = _context.Photo.Find(id);
        
        if (photo == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Photo not found"
            });
        }
        
        return Ok(new PhotoViewModel(photo));
    }

    [HttpPost]
    public IActionResult PhotoCreate([FromBody] PhotoViewModel photoViewModel)
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
        
        // Create a new photo from the view model
        var createdPhoto = Photo.FromViewModel(photoViewModel);
        
        createdPhoto.MemberCreatedId = _currentUser.Id;
        
        // Add the photo to the database
        _context.Photo.Add(createdPhoto);
        _context.SaveChanges();
        
        // Return the created photo
        return Created($"/api/v1/ReceiptPhoto/{createdPhoto.Id}", new
        {
            status = 201,
            message = "Photo created",
            photo = new PhotoViewModel(createdPhoto)
        });
    }

    
    [HttpPut("{id}")]
    [ActionName("photoUpdate")]
    public IActionResult PhotoUpdate(long id, [FromBody] PhotoViewModel updatedPhotoViewModel)
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
        if (updatedPhotoViewModel.Id != null && updatedPhotoViewModel.Id != id)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Invalid request body, ID in URL does not match ID in request body"
            });
        }

        // Get the ID from URL (/ReceiptPhoto/{id}/) if it was not in the body
        if (updatedPhotoViewModel.Id == null)
        {
            updatedPhotoViewModel.Id = id;
        }

        // Retrieve the existing photo from your data store (e.g., database)
        Photo? existingPhoto = _context.Photo.Find(id);

        // Check if the photo with the provided ID exists
        if (existingPhoto == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Photo not found"
            });
        }

        if (PhotosAreEqual(existingPhoto, updatedPhotoViewModel))
        {
            // If the data is the same, return a response indicating no update was performed
            return Ok(new {
                status = 200,
                message = "No changes were made to the photo"
            });
        }

        if (updatedPhotoViewModel.Base64Image != null)
        {
            existingPhoto.Base64Image = updatedPhotoViewModel.Base64Image;
        }

        if (updatedPhotoViewModel.fileName != null)
        {
            existingPhoto.fileName = updatedPhotoViewModel.fileName;
        }

        if (updatedPhotoViewModel.fileExtension != null)
        {
            existingPhoto.fileExtension = updatedPhotoViewModel.fileExtension;
        }

        if (updatedPhotoViewModel.ReceiptId != null)
        {
            existingPhoto.ReceiptId = updatedPhotoViewModel.ReceiptId;
        }
        
        existingPhoto.DateTimeModified = DateTime.UtcNow;

        // Save the changes to your data store (e.g., update the database record)
        _context.Update(existingPhoto);
        _context.SaveChanges();

        return Ok();
    }

    
    [HttpDelete("{id}")]
    [ActionName("photoDelete")]
    public IActionResult PhotoDelete(long id)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();
        
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
        
        // Try to remove the photo from your data store and handle if it is not possible
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
                message = "Foto kon niet worden verwijderd" // TODO Check which dependency is causing the conflict
            });
        }
        
        return NoContent();
    }
    
    
    private bool PhotosAreEqual(Photo existingPhoto, PhotoViewModel updatedPhotoViewModel)
    {
        // Compare relevant properties to check if the photo is unchanged

        // Check if given value is null or if they are equal
        bool nameEqual = (updatedPhotoViewModel.fileName == null)
                         || (existingPhoto.fileName == updatedPhotoViewModel.fileName);

        // Check if given value is null or if they are equal
        bool extEqual = (updatedPhotoViewModel.fileExtension == null)
                        || (existingPhoto.fileExtension == updatedPhotoViewModel.fileExtension);
        
        // Check if given value is null or if they are equal
        bool dataEqual = (updatedPhotoViewModel.Base64Image == null)
                         || (existingPhoto.Base64Image == updatedPhotoViewModel.Base64Image);

        // Check if given value is null or if they are equal
        bool receiptEqual = (updatedPhotoViewModel.ReceiptId == null)
                         || (existingPhoto.ReceiptId == updatedPhotoViewModel.ReceiptId);

        // Compare other properties as needed
        return nameEqual
               && extEqual
               && dataEqual
               && receiptEqual;
    }
}