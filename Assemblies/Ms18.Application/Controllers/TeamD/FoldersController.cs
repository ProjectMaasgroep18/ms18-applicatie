using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ms18.Application.Interface.TeamD;
using Ms18.Database.Models.TeamD.PhotoAlbum;

namespace Ms18.Application.Controllers.TeamD;

[ApiController]
[Route("api/photo-album")]
public class FoldersController : ControllerBase
{
    private readonly IPhotoRepository _photoRepository;
    private readonly IFolderRepository _folderRepository;
    private readonly ILogger<FoldersController> _logger;

    public FoldersController(IPhotoRepository photoRepository, IFolderRepository folderRepository, ILogger<FoldersController> logger)
    {
        _photoRepository = photoRepository;
        _folderRepository = folderRepository;
        _logger = logger;
    }

    [HttpPost("create-folder")]
    public async Task<IActionResult> CreateFolder([FromBody] Folder model)
    {
        try
        {
            var folder = await _folderRepository.CreateFolder(model);
            return Created("", folder);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error occurred while creating a folder.");
            return BadRequest("Error creating folder: " + ex.InnerException?.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a folder.");
            return StatusCode(500, "An internal error occurred.");
        }
    }
}

