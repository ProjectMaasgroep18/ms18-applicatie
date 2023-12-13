using Maasgroep.Database.Context;
using Maasgroep.Database.team_d.Models;
using Microsoft.EntityFrameworkCore;
using ms18_applicatie.Interfaces.team_d;

namespace ms18_applicatie.Repository.team_d;

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

