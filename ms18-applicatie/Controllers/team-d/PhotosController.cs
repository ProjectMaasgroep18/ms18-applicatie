using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Interfaces;
using ms18_applicatie.Models.team_d;

namespace ms18_applicatie.Controllers.team_d;

[ApiController]
[Route("api/photos")]
public class PhotosController : ControllerBase
{
    private readonly IPhotoRepository _photoRepository;
    private readonly ILogger<PhotosController> _logger;

    public PhotosController(IPhotoRepository photoRepository, ILogger<PhotosController> logger)
    {
        _photoRepository = photoRepository;
        _logger = logger;
    }
    [HttpPost]
    [ProducesResponseType(typeof(CreateResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UploadPhoto([FromBody] PhotoUploadModel uploadModel)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var uploaderId = 1L; // TODO Get from authentication
        var needsApproval = false; // TODO Get from authentication

        try
        {
            var photoId = await _photoRepository.AddPhoto(uploaderId, needsApproval, uploadModel);

            if (photoId == null || photoId == Guid.Empty)
            {
                _logger.LogError("Failed to upload photo");
                return StatusCode(500, "An error occured while uploading the photo,");
            }

            return CreatedAtRoute("GetPhoto", new { id = photoId }, new CreateResponseModel { Id = (Guid)photoId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while uploading the photo.");
            return StatusCode(500, "An error occurred while uploading the photo.");
        }
    }

    [HttpGet("{id}", Name = "GetPhoto")]
    [ProducesResponseType(typeof(PhotoViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PhotoViewModel>> GetPhoto([FromRoute] Guid id)
    {
        try
        {
            var photoViewModel = await _photoRepository.GetPhotoViewModelById(id);
            if (photoViewModel == null)
            {
                return NotFound($"Photo with ID {id} not found.");
            }

            return Ok(photoViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving photo with ID {id}.");
            return StatusCode(500, "An error occurred while retrieving the photo.");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePhoto([FromRoute] Guid id, [FromBody] PhotoUpdateModel updateModel)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var photo = await _photoRepository.GetPhotoById(id);
            if (photo == null)
            {
                return NotFound($"Photo with ID {id} not found.");
            }

            photo.Title = updateModel.Title;
            photo.ContentType = updateModel.ContentType;
            photo.TakenOn = updateModel.TakenOn;
            photo.Location = updateModel.Location;
            photo.AlbumLocationId = updateModel.AlbumLocationId;
            photo.NeedsApproval = updateModel.NeedsApproval;

            await _photoRepository.UpdatePhoto(photo);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while updating photo with ID {id}.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the photo.");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePhoto([FromRoute] Guid id)
    {
        try
        {
            var deleteSuccess = await _photoRepository.DeletePhoto(id);

            if (!deleteSuccess) return NotFound($"Photo with ID {id} not found");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while deleting photo with ID {id}.");
            return StatusCode(500, "An error occurred while deleting the photo.");
        }
    }

    [HttpGet("{id}/download")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DownloadPhoto([FromRoute] Guid id)
    {
        try
        {
            var photo = await _photoRepository.GetPhotoById(id);
            if (photo == null)
            {
                return NotFound($"Photo with ID {id} not found.");
            }

            var imageStream = new MemoryStream(photo.ImageData);
            return File(imageStream, photo.ContentType, photo.Title ?? "downloaded_image");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while downloading photo with ID {id}.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while downloading the photo.");
        }
    }

    [HttpGet("album/{albumId}")]
    [ProducesResponseType(typeof(PaginatedResponseModel<PhotoViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginatedResponseModel<PhotoViewModel>>> ListPhotosInAlbum(
        [FromRoute] Guid albumId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Invalid page number or page size.");
        }

        try
        {
            var (photos, totalCount) = await _photoRepository.GetPhotosByAlbumId(albumId, pageNumber, pageSize);

            var photoViewModels = photos.Select(p => new PhotoViewModel
            {
                Id = p.Id,
                UploaderId = p.UploaderId,
                UploadDate = p.UploadDate,
                Title = p.Title,
                ImageBase64 = Convert.ToBase64String(p.ImageData),
                ContentType = p.ContentType,
                TakenOn = p.TakenOn,
                Location = p.Location,
                AlbumLocationId = p.AlbumLocationId,
                LikesCount = p.Likes?.Count() ?? 0
            }).ToList();

            var response = new PaginatedResponseModel<PhotoViewModel>
            {
                Items = photoViewModels,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving photos for album ID {albumId}.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving photos.");
        }
    }
}
