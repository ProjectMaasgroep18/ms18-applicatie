using Microsoft.AspNetCore.Mvc;
using Ms18.Application.Interface.TeamD;
using Ms18.Application.Models.TeamD;

namespace Ms18.Application.Controllers.TeamD;

[ApiController]
[Route("api/photo-album")]
public class PhotosController : ControllerBase
{
    private readonly IPhotoRepository _photoRepository;
    private readonly IFolderRepository _folderRepository;

    public PhotosController(IPhotoRepository photoRepository, IFolderRepository folderRepository)
    {
        _photoRepository = photoRepository;
        _folderRepository = folderRepository;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadPhoto([FromBody] PhotoUploadModel model)
    {
        // Validate the incoming data (e.g., check if the album exists)
        var folderExists = await _folderRepository.FolderExists(model.FolderLocationId);

        if (!folderExists)
        {
            return NotFound($"Album with ID {model.FolderLocationId} not found.");
        }

        // Use the repository to add the photo
        var photo = await _photoRepository.AddPhoto(model, 1);  // TODO: Set the uploader's ID based on the authenticated user

        return Ok(new { photo.Id });
    }
}

