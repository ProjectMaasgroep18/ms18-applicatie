using Maasgroep.Database.Context.team_d.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ms18_applicatie.Interfaces.team_d;

namespace ms18_applicatie.Controllers.team_d;

[ApiController]
[Route("api/photo-album")]
public class AlbumsController : ControllerBase
{
    private readonly IPhotoRepository _photoRepository;
    private readonly IAlbumRepository _albumRepository;
    private readonly ILogger<AlbumsController> _logger;

    public AlbumsController(IPhotoRepository photoRepository, IAlbumRepository albumRepository, ILogger<AlbumsController> logger)
    {
        _photoRepository = photoRepository;
        _albumRepository = albumRepository;
        _logger = logger;
    }

    [HttpPost("create-album")]
    public async Task<IActionResult> CreateAlbum([FromBody] Album model)
    {
        try
        {
            var album = await _albumRepository.CreateAlbum(model);
            return Created("", album);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error occurred while creating an album.");
            return BadRequest("Error creating album: " + ex.InnerException?.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a album.");
            return StatusCode(500, "An internal error occurred.");
        }
    }
}

