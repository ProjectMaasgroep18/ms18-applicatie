using Microsoft.AspNetCore.Mvc;
using Maasgroep.Interfaces;
using Maasgroep.Models.team_d;

namespace Maasgroep.Controllers.team_d;

[ApiController]
[Route("api/tags")]
public class TagsController : ControllerBase
{
    private readonly ITagsRepository _tagsRepository;
    private readonly ILogger<TagsController> _logger;

    public TagsController(ITagsRepository tagsRepository, ILogger<TagsController> logger)
    {
        _tagsRepository = tagsRepository;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagModel? createTagModel)
    {
        try
        {
            if (createTagModel == null || string.IsNullOrWhiteSpace(createTagModel.Name))
            {
                return BadRequest("Tag name is required.");
            }

            var createdTag = await _tagsRepository.CreateTagAsync(createTagModel.Name);

            if (createdTag == null)
            {
                return StatusCode(500, "There was a problem while creating the tag.");
            }

            return CreatedAtAction(nameof(GetTag), new { id = createdTag.Id }, createdTag);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a tag.");
            return StatusCode(500, "An error occurred while creating the tag.");
        }
    }

    [HttpGet("{tagId}")]
    public async Task<IActionResult> GetTag([FromRoute] Guid tagId)
    {
        try
        {
            var tag = await _tagsRepository.GetTagByIdAsync(tagId);

            if (tag == null)
            {
                return NotFound($"Tag with ID {tagId} not found.");
            }

            return Ok(tag);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving tag with ID {tagId}.");
            return StatusCode(500, "An error occurred while retrieving the tag.");
        }
    }

    [HttpDelete("{tagId}")]
    public async Task<IActionResult> DeleteTag([FromRoute] Guid tagId)
    {
        try
        {
            var result = await _tagsRepository.DeleteTagAsync(tagId);
            if (!result)
            {
                return NotFound($"Tag with ID {tagId} not found.");
            }

            return Ok($"Tag with ID {tagId} deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while deleting tag with ID {tagId}.");
            return StatusCode(500, "An error occurred while deleting the tag.");
        }
    }

    [HttpPut("{tagId}")]
    public async Task<IActionResult> UpdateTag([FromRoute] Guid tagId, [FromBody] TagUpdateModel? tagUpdateModel)
    {
        try
        {
            if (tagUpdateModel == null || string.IsNullOrWhiteSpace(tagUpdateModel.Name))
            {
                return BadRequest("Tag name is required.");
            }

            var updatedTag = await _tagsRepository.UpdateTagAsync(tagId, tagUpdateModel.Name);

            if (updatedTag == null)
            {
                return NotFound($"Tag with ID {tagId} not found.");
            }

            return Ok(updatedTag);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while updating tag with ID {tagId}.");
            return StatusCode(500, "An error occurred while updating the tag.");
        }
    }

    [HttpPost("album/{albumId}/tag/{tagId}")]
    public async Task<IActionResult> AddTagToAlbum([FromRoute] Guid albumId, [FromRoute] Guid tagId)
    {
        try
        {
            var result = await _tagsRepository.AddTagToAlbumAsync(albumId, tagId);
            if (!result)
            {
                return NotFound("Album or Tag not found.");
            }

            return Ok("Tag added to album successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding tag to album.");
            return StatusCode(500, "An error occurred while adding the tag to the album.");
        }
    }

    [HttpDelete("album/{albumId}/tag/{tagId}")]
    public async Task<IActionResult> RemoveTagFromAlbum([FromRoute] Guid albumId, [FromRoute] Guid tagId)
    {
        try
        {
            var result = await _tagsRepository.RemoveTagFromAlbumAsync(albumId, tagId);
            if (!result)
            {
                return NotFound("Album or Tag not found.");
            }

            return Ok("Tag removed from album successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while removing tag from album.");
            return StatusCode(500, "An error occurred while removing the tag from the album.");
        }
    }

    [HttpGet("albums/{tagId}")]
    public async Task<IActionResult> GetAlbumsWithTag([FromRoute] Guid tagId)
    {
        try
        {
            var albums = await _tagsRepository.GetAlbumsWithTagAsync(tagId);
            if (albums == null || !albums.Any())
            {
                return NotFound("No albums found with the specified tag.");
            }

            return Ok(albums);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving albums with tag.");
            return StatusCode(500, "An error occurred while retrieving albums.");
        }
    }
}