using Maasgroep.Database.Context.Tables.PhotoAlbum;
using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Interfaces;

namespace ms18_applicatie.Controllers.team_d;

[ApiController]
[Route("api/albums")]
public class AlbumsController : ControllerBase
{
    private readonly IAlbumRepository _albumRepository;
    private readonly ILogger<AlbumsController> _logger;

    public AlbumsController(IAlbumRepository albumRepository, ILogger<AlbumsController> logger)
    {
        _albumRepository = albumRepository;
        _logger = logger;
    }

    [HttpPost("create-album")]
    public async Task<ActionResult<Album>> CreateAlbum ([FromBody] Album? album)
    {
        if (album == null) return BadRequest("Album data is null.");
        if (album.Id == Guid.Empty) album.Id = Guid.NewGuid();

        try
        {
            var exists = await _albumRepository.AlbumExists(album.Name, album.ParentAlbumId);
            if (exists) return BadRequest($"An album with the name '{album.Name}' already exists in the specified parent album.");

            await _albumRepository.AddAlbum(album);

            var albumUrl = Url.Action("GetAlbum", new { id = album.Id });
            if (string.IsNullOrEmpty(albumUrl))
            {
                _logger.LogError($"Failed to generate URL for newly created album with ID {album.Id}");
                return StatusCode(500, "An error occurred while creating the album.");
            }
            return Created(albumUrl, album);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating the album.");
            return StatusCode(500, "An error occurred while creating the album.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Album>> GetAlbum(Guid id)
    {
        // This doesn't get the photos for the album that is in the PhotosController since it has to be paginated.
        try
        {
            var album = await _albumRepository.GetAlbumById(id);
            if (album == null)
            {
                return NotFound($"Album with ID {id} not found.");
            }

            return Ok(album);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving album with ID {id}.");
            return StatusCode(500, "An internal error occurred.");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Album>>> GetAllAlbums()
    {
        try
        {
            var albums = await _albumRepository.GetAllAlbums();
            return Ok(albums);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving albums.");
            return StatusCode(500, "An internal error occurred.");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAlbum(Guid id, [FromBody] Album? updatedAlbum)
    {
        if (updatedAlbum == null)
        {
            return BadRequest("Album data is null.");
        }

        try
        {
            var album = await _albumRepository.GetAlbumById(id);
            if (album == null)
            {
                return NotFound($"Album with ID {id} not found.");
            }

            album.Name = updatedAlbum.Name;
            album.ParentAlbumId = updatedAlbum.ParentAlbumId;

            await _albumRepository.UpdateAlbum(album);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while updating album with ID {id}.");
            return StatusCode(500, "An internal error occurred.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAlbum(Guid id)
    {
        try
        {
            var album = await _albumRepository.GetAlbumById(id);
            if (album == null)
            {
                return NotFound($"Album with ID {id} not found.");
            }

            await _albumRepository.DeleteAlbum(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while deleting album with ID {id}.");
            return StatusCode(500, "An internal error occurred.");
        }
    }
}

