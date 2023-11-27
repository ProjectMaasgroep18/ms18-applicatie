using Microsoft.AspNetCore.Mvc;
using Ms18.Application.Interface.TeamD;
using Ms18.Application.Models.TeamD;

namespace Ms18.Application.Controllers.TeamD;

[ApiController]
[Route("api/photo-album")]
public class FoldersController : ControllerBase
{
    private readonly IPhotoRepository _photoRepository;
    private readonly IFolderRepository _folderRepository;

    public FoldersController(IPhotoRepository photoRepository, IFolderRepository folderRepository)
    {
        _photoRepository = photoRepository;
        _folderRepository = folderRepository;
    }

    [HttpPost("create-folder")]
    public async Task<IActionResult> CreateFolder([FromBody] CreateFolderModel model)
    {
        var folder = await _folderRepository.CreateFolder(model.Name, model.ParentFolderId);
        return Created("", folder);
    }
}

