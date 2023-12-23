using Maasgroep.Database.Context.Tables.PhotoAlbum;
using Microsoft.AspNetCore.Mvc;
using Maasgroep.Interfaces;

namespace Maasgroep.Controllers.team_d;

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
    public async Task<IActionResult> AddLike([FromRoute] Guid photoId, [FromRoute] long userId)
    {
        try
        {
            var newLike = new Like
            {
                PhotoId = photoId,
                MemberId = userId,
                LikedOn = DateTime.UtcNow
            };

            await _likesRepository.AddLike(newLike);

            return Ok("Like added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while adding a like to photo with ID {photoId}.");
            return StatusCode(500, "An error occurred while adding the like.");
        }
    }

    [HttpDelete("{photoId}/{userId}")]
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

            return Ok("Like deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while deleting a like for photo with ID {photoId} and user ID {userId}.");
            return StatusCode(500, "An error occurred while deleting the like.");
        }
    }

    [HttpGet("photo/{photoId}")]
    public async Task<IActionResult> GetAllLikesForPhoto([FromRoute] Guid photoId)
    {
        try
        {
            var likes = await _likesRepository.GetAllLikesForPhoto(photoId);
            if (likes == null || !likes.Any())
            {
                return NotFound("No likes found for this photo.");
            }

            return Ok(likes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving likes for photo with ID {photoId}.");
            return StatusCode(500, "An error occurred while retrieving likes.");
        }
    }

    [HttpGet("most-liked")]
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