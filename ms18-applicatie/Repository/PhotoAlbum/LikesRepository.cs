using Maasgroep.Database;
using Maasgroep.Database.Context.Tables.PhotoAlbum;
using Microsoft.EntityFrameworkCore;
using ms18_applicatie.Interfaces;
using ms18_applicatie.Models.team_d;

namespace ms18_applicatie.Repository.PhotoAlbum;

public class LikesRepository : ILikesRepository
{
    private readonly MaasgroepContext _context;

    public LikesRepository(MaasgroepContext context)
    {
        _context = context;
    }
    public async Task<Guid?> AddLike(Like like)
    {
        _context.Likes.Add(like);
        var affectedRows = await _context.SaveChangesAsync();

        if (affectedRows > 0 && like.Id != Guid.Empty) return like.Id;
        return null;
    }

    public async Task DeleteLike(Guid likeId)
    {
        var like = await _context.Likes.FindAsync(likeId);
        if (like != null)
        {
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<LikeViewModel>> GetAllLikesForPhoto(Guid photoId)
    {
        return await _context.Likes
            .Where(like => like.PhotoId == photoId)
            .Select(l => new LikeViewModel
            {
                Id = l.Id,
                MemberId = l.MemberId,
                PhotoId = l.PhotoId,
                LikedOn = l.LikedOn,
            })
            .ToListAsync();
    }

    public async Task<Like?> GetLike(Guid photoId, long userId)
    {
        return await _context.Likes
            .Where(l => l.PhotoId == photoId && l.MemberId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<PhotoViewModel>> GetTopLikedPhotos(DateTime startDate, DateTime endDate, int topCount)
    {
        return await _context.Likes
            .Where(like => like.LikedOn >= startDate && like.LikedOn <= endDate)
            .GroupBy(like => like.PhotoId)
            .OrderByDescending(g => g.Count())
            .Take(topCount)
            .Select(g => g.Key)
            .Join(_context.Photos, photoId => photoId, photo => photo.Id, (photoId, photo) => photo)
            .Select(p => new PhotoViewModel
        {
                Id = p.Id,
                UploaderId = p.UploaderId,
                UploadDate = p.UploadDate,
                Title = p.Title,
                ImageBase64 = Convert.ToBase64String(p.ImageData),
                ContentType = p.ContentType,
                TakenOn = p.TakenOn,
                Location = p.Location,
                AlbumLocationId = p.AlbumLocationId,
                LikesCount = p.Likes.Count()
            })
            .ToListAsync();
    }
}
