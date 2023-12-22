using Maasgroep.Database.Context.Tables.PhotoAlbum;
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
    [Route("upload")]
    public async Task<IActionResult> UploadPhoto([FromBody] PhotoUploadModel uploadModel)
    {
        if (string.IsNullOrEmpty(uploadModel.ImageBase64) || uploadModel.AlbumId == Guid.Empty || string.IsNullOrEmpty(uploadModel.ContentType))
        {
            return BadRequest("Photo, content type and album ID are required.");
        }

        try
        {
            var photoModel = await CreatePhotoModel(uploadModel);
            await _photoRepository.AddPhoto(photoModel);
            return Ok("Photo uploaded successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while uploading the photo.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while uploading the photo.");
        }
    }

    private async Task<Photo> CreatePhotoModel(PhotoUploadModel uploadModel)
    {
        var imageBytes = Convert.FromBase64String(uploadModel.ImageBase64);

        var photoModel = new Photo
        {
            Id = Guid.NewGuid(),
            UploaderId = 1, // TODO: Replace with actual authenticated user ID
            UploadDate = DateTime.UtcNow,
            Title = uploadModel.Title,
            ImageData = imageBytes,
            AlbumLocationId = uploadModel.AlbumId,
            TakenOn = uploadModel.TakenOn,
            Location = uploadModel.Location,
            ContentType = uploadModel.ContentType
        };

        return photoModel;
    }

    [HttpGet("{photoId}")]
    public async Task<ActionResult<PhotoViewModel>> GetPhotoById(Guid photoId)
    {
        try
        {
            var photo = await _photoRepository.GetPhotoById(photoId);
            if (photo == null)
            {
                return NotFound($"Photo with ID {photoId} not found.");
            }

            var photoViewModel = new PhotoViewModel
            {
                Id = photo.Id,
                UploaderId = photo.UploaderId,
                UploadDate = photo.UploadDate,
                Title = photo.Title,
                ImageBase64 = Convert.ToBase64String(photo.ImageData),
                ContentType = photo.ContentType,
                AlbumLocationId = photo.AlbumLocationId,
                TakenOn = photo.TakenOn,
                Location = photo.Location,
                LikesCount = photo.Likes?.Count() ?? 0
            };

            return Ok(photoViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving photo with ID {photoId}.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the photo.");
        }
    }

    [HttpPut("{photoId}")]
    public async Task<IActionResult> UpdatePhoto(Guid photoId, [FromBody] PhotoUpdateModel updateModel)
    {
        try
        {
            var existingPhoto = await _photoRepository.GetPhotoById(photoId);
            if (existingPhoto == null)
            {
                return NotFound($"Photo with ID {photoId} not found.");
            }
            existingPhoto.Title = updateModel.Title;
            existingPhoto.TakenOn = updateModel.TakenOn;
            existingPhoto.Location = updateModel.Location;
            existingPhoto.AlbumLocationId = updateModel.AlbumLocationId;


            await _photoRepository.UpdatePhoto(existingPhoto);
            return Ok("Photo updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while updating photo with ID {photoId}.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the photo.");
        }
    }

    [HttpDelete("{photoId}")]
    public async Task<IActionResult> DeletePhoto(Guid photoId)
    {
        try
        {
            var photo = await _photoRepository.GetPhotoById(photoId);
            if (photo == null)
            {
                return NotFound($"Photo with ID {photoId} not found.");
            }

            await _photoRepository.DeletePhoto(photoId);
            return Ok($"Photo with ID {photoId} deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while deleting photo with ID {photoId}.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the photo.");
        }
    }

    [HttpGet("{photoId}/download")]
    public async Task<IActionResult> DownloadPhoto(Guid photoId)
    {
        try
        {
            var photo = await _photoRepository.GetPhotoById(photoId);
            if (photo == null)
            {
                return NotFound($"Photo with ID {photoId} not found.");
            }

            var imageStream = new MemoryStream(photo.ImageData);
            return File(imageStream, photo.ContentType, photo.Title ?? "downloaded_image");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while downloading photo with ID {photoId}.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while downloading the photo.");
        }
    }

    [HttpGet("album/{albumId}/photos")]
    public async Task<ActionResult<PaginatedResponseModel<PhotoViewModel>>> ListPhotosInAlbum(Guid albumId, int pageNumber = 1, int pageSize = 10)
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
