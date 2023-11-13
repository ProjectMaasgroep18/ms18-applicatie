using Maasgroep.Database;
using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Models.team_d;
using ms18_applicatie.Repository.PhotoAlbum;

namespace ms18_applicatie.Controllers;

[ApiController]
[Route("api/photo-album")]
public class PhotoAlbumController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PhotoAlbumRepository _photoAlbumRepository;

    public PhotoAlbumController(PhotoAlbumRepository photoAlbumRepository, ILogger<HomeController> logger)
    {
        _photoAlbumRepository = photoAlbumRepository;
        _logger = logger;
    }

    [HttpPost("create-folder")]
    public async Task<IActionResult> CreateFolder([FromBody] CreateFolderModel model)
    {
        var folder = await _photoAlbumRepository.CreateFolder(model.Name, model.ParentFolderId);
        return Created("", folder);
    }

    [HttpDelete("delete-folder/{id}")]
    public async Task<IActionResult> DeleteFolder(Guid id)
    {
        bool success = await _photoAlbumRepository.DeleteFolder(id);
        if (!success)
        {
            return NotFound($"Folder with ID {id} not found or could not be deleted.");
        }
        return NoContent();
    }

    [HttpPut("rename-folder/{id}")]
    public async Task<IActionResult> RenameFolder(Guid id, [FromBody] RenameFolderModel model)
    {
        bool success = await _photoAlbumRepository.RenameFolder(id, model.NewName);
        if (!success)
        {
            return NotFound($"Folder with ID {id} not found or could not be renamed.");
        }
        return Ok();
    }

    [HttpPut("move-folder/{id}")]
    public async Task<IActionResult> MoveFolder(Guid id, [FromBody] MoveFolderModel model)
    {
        bool success = await _photoAlbumRepository.MoveFolder(id, model.NewParentFolderId);
        if (!success)
        {
            return NotFound($"Folder with ID {id} not found or could not be moved.");
        }
        return Ok();
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadPhoto([FromBody] PhotoUploadModel model)
    {
        // Validate the incoming data (e.g., check if the album exists)
        var foldereExists = await _photoAlbumRepository.AlbumExists(model.FolderLocationId);
        if (!foldereExists)
        {
            return NotFound($"Album with ID {model.FolderLocationId} not found.");
        }

        // Use the repository to add the photo
        var photo = await _photoAlbumRepository.AddPhoto(model, 1);  // TODO: Set the uploader's ID based on the authenticated user

        return Ok(new { photo.Id });
    }

    [HttpGet("photos-in-directory")]
    public async Task<IActionResult> GetPhotosInDirectory(
        [FromQuery] Guid folderLocationId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        // Retrieve photos in the specified directory with paging
        var result = await _photoAlbumRepository.GetPhotosInDirectory(folderLocationId, page, pageSize);

        return Ok(result);
    }

    [HttpGet("folders-in-directory")]
    public async Task<IActionResult> GetFoldersInDirectory([FromQuery] Guid? folderLocationId)
    {
        // Retrieve folders in the specified directory or all folders with no parent folder
        var folders = await _photoAlbumRepository.GetFoldersInDirectory(folderLocationId);

        return Ok(folders);
    }
}
