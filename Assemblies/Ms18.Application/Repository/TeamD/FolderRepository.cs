using Microsoft.EntityFrameworkCore;
using Ms18.Application.Interface.TeamD;
using Ms18.Database;
using Ms18.Database.Models.TeamD.PhotoAlbum;

namespace Ms18.Application.Repository.TeamD;

public class FolderRepository : IFolderRepository
{
    private readonly MaasgroepContext _context;

    public FolderRepository(MaasgroepContext context)
    {
        _context = context;
    }

    public async Task<bool> FolderExists(Guid folderId)
    {
        return await _context.Folders.AnyAsync(f => f.Id == folderId);
    }

    public async Task<Folder> CreateFolder(Folder folder)
    {
        if (folder.Id == Guid.Empty) folder.Id = Guid.NewGuid();

        _context.Folders.Add(folder);
        await _context.SaveChangesAsync();
        return folder;
    }
}

