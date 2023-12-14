using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Interfaces.team_d;
using ms18_applicatie.Models.team_d;

namespace ms18_applicatie.Controllers.team_d;

[ApiController]
[Route("api/photo-album")]
public class PhotosController : ControllerBase
{
    private readonly IPhotoRepository _photoRepository;
    private readonly IAlbumRepository _albumRepository;
    private readonly ILogger<PhotosController> _logger;

    public PhotosController(IPhotoRepository photoRepository, IAlbumRepository albumRepository, ILogger<PhotosController> logger)
    {
        _photoRepository = photoRepository;
        _albumRepository = albumRepository;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadPhoto([FromBody] PhotoUploadModel model)
    {
        //try
        //{
        //    var albumExists = await _albumRepository.AlbumExists(model.AlbumLocationId);

        //    if (!albumExists)
        //    {
        //        return NotFound($"Album with ID {model.AlbumLocationId} not found.");
        //    }

        //    var photo = await _photoRepository.AddPhoto(model,
        //        1); // TODO: Set the uploader's ID based on the authenticated user

        //    if (photo == null)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading photo");
        //    }

        //    return Ok(new { photo.Id });
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError(ex, "Unhandled exception in UploadPhoto");
        //    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        //}
        return Ok();
    }
}
