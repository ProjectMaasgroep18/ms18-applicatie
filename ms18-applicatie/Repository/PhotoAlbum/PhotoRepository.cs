using Maasgroep.Database;
using Maasgroep.Database.Context.Tables.PhotoAlbum;
using Microsoft.EntityFrameworkCore;
using Maasgroep.Interfaces;

namespace Maasgroep.Repository.PhotoAlbum;

public class PhotoRepository : IPhotoRepository
{
    private readonly MaasgroepContext _context;

    public PhotoRepository(MaasgroepContext context)
    {
        _context = context;
    }

    public async Task AddPhoto(Photo photo)
    {
        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePhoto(Guid photoId)
    {
        var photo = await _context.Photos.FindAsync(photoId);
        if (photo != null)
        {
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Photo?> GetPhotoById(Guid photoId)
    {
        return await _context.Photos
            .Include(p => p.AlbumLocation)
            .Include(p => p.Likes)         
            .FirstOrDefaultAsync(p => p.Id == photoId);
    }

    public async Task<(IEnumerable<Photo> Photos, int TotalCount)> GetPhotosByAlbumId(Guid albumId, int pageNumber, int pageSize)
    {
        var query = _context.Photos.Where(p => p.AlbumLocationId == albumId);

        var totalCount = await query.CountAsync();
        var photos = await query.OrderBy(p => p.UploadDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (photos, totalCount);
    }

    public async Task UpdatePhoto(Photo photo)
    {
        _context.Entry(photo).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}

