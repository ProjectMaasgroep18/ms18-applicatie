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

    public async Task<bool> AlbumExists(Guid albumId)
    {
        return await _context.Albums.AnyAsync(f => f.Id == albumId);
    }

    public async Task<Album> CreateAlbum(Album album)
    {
        if (album.Id == Guid.Empty) album.Id = Guid.NewGuid();

        _context.Albums.Add(album);
        await _context.SaveChangesAsync();
        return album;
    }
}

