using Maasgroep.Database.Context;
using Maasgroep.Database.Context.team_d.Models;
using Microsoft.EntityFrameworkCore;
using ms18_applicatie.Interfaces.team_d;

namespace ms18_applicatie.Repository.team_d;

public class AlbumRepository : IAlbumRepository
{
    private readonly MaasgroepContext _context;

    public AlbumRepository(MaasgroepContext context)
    {
        _context = context;
    }

    public async Task<bool> AlbumExists(string albumName, Guid? parentAlbumId)
    {
        return await _context.Albums.AnyAsync(a => a.Name == albumName && a.ParentAlbumId == parentAlbumId);
    }

    public async Task AddAlbum(Album album)
    {
        _context.Albums.Add(album);
        await _context.SaveChangesAsync();
    }

    public async Task<Album?> GetAlbumById(Guid id)
    {
        return await _context.Albums
            .Include(a => a.ChildAlbums)
            .Include(a => a.AlbumTags)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Album>> GetAllAlbums()
    {
        return await _context.Albums.ToListAsync();
    }

    public async Task UpdateAlbum(Album album)
    {
        _context.Albums.Update(album);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAlbum(Guid albumId)
    {
        var album = await _context.Albums.FindAsync(albumId);

        if (album != null)
        {
            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
        }
    }
}

