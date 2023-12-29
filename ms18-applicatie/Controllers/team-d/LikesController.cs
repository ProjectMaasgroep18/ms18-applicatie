using Maasgroep.Database.Context.Tables.PhotoAlbum;
using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Attributes;
using ms18_applicatie.Interfaces;
using ms18_applicatie.Models.team_d;

namespace ms18_applicatie.Controllers.team_d;

[ApiController]
[Route("api/likes")]
public class LikesController : ControllerBase
{
    private readonly ILikesRepository _likesRepository;
    private readonly ILogger<LikesController> _logger;

    public LikesController(ILikesRepository likesRepository, ILogger<LikesController> logger)
    {
        _likesRepository = likesRepository;
        _logger = logger;
    }


    [HttpPost("{photoId}/{userId}")]
    [PhotoAlbumAuthorization("photoAlbum")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(StatusCodes.Status409Conflict)] 
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> AddLike([FromRoute] Guid photoId, [FromRoute] long userId)
    {
        try
        {
            var existingLike = await _likesRepository.GetLike(photoId, userId);
            if (existingLike != null)
            {
                return StatusCode(409, "Like already exists.");
            }

            var newLike = new Like
            {
                PhotoId = photoId,
                MemberId = userId,
                LikedOn = DateTime.UtcNow
            };

            var likeId = await _likesRepository.AddLike(newLike);

            if (likeId == null || likeId == Guid.Empty)
            {
                _logger.LogError("Failed to create like");
                return StatusCode(500, "An error occurred while adding the like.");
            }

            return StatusCode(201, "Like added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while adding a like to photo with ID {photoId}.");
            return StatusCode(500, "An error occurred while adding the like.");
        }
    }

    [HttpDelete("{photoId}/{userId}")]
    [PhotoAlbumAuthorization("photoAlbum")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteLike([FromRoute] Guid photoId, [FromRoute] long userId)
    {
        try
        {
            var likeToDelete = await _likesRepository.GetLike(photoId, userId);
            if (likeToDelete == null)
            {
                return NotFound("Like not found.");
            }

            await _likesRepository.DeleteLike(likeToDelete.Id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while deleting a like for photo with ID {photoId} and user ID {userId}.");
            return StatusCode(500, "An error occurred while deleting the like.");
        }
    }

    [HttpGet("photo/{photoId}")]
    [PhotoAlbumAuthorization("photoAlbum")]
    [ProducesResponseType(typeof(IEnumerable<LikeViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllLikesForPhoto([FromRoute] Guid photoId)
    {
        try
        {
            var likes = await _likesRepository.GetAllLikesForPhoto(photoId);

            return Ok(likes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving likes for photo with ID {photoId}.");
            return StatusCode(500, "An error occurred while retrieving likes.");
        }
    }

    [HttpGet("most-liked")]
    [PhotoAlbumAuthorization("photoAlbum")]
    [ProducesResponseType(typeof(IEnumerable<PhotoViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTopLikedPhotos([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int topCount)
    {
        try
        {
            var photos = await _likesRepository.GetTopLikedPhotos(startDate, endDate, topCount);
            if (photos == null || !photos.Any())
            {
                return NotFound("No liked photos found in the given date range.");
            }

            return Ok(photos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving top liked photos.");
            return StatusCode(500, "An error occurred while retrieving the data.");
        }
    }
}