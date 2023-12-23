using Maasgroep.Database;
using Maasgroep.Database.Context.Tables.PhotoAlbum;
using Microsoft.EntityFrameworkCore;
using ms18_applicatie.Interfaces;

namespace ms18_applicatie.Repository.PhotoAlbum;

public class LikesRepository : ILikesRepository
{
    private readonly MaasgroepContext _context;

    public LikesRepository(MaasgroepContext context)
    {
        _context = context;
    }
    public async Task AddLike(Like like)
    {
        _context.Likes.Add(like);
        await _context.SaveChangesAsync();
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

    public async Task<IEnumerable<Like>> GetAllLikesForPhoto(Guid photoId)
    {
        return await _context.Likes
            .Where(like => like.PhotoId == photoId)
            .ToListAsync();
    }

    public async Task<Like?> GetLike(Guid photoId, long userId)
    {
        return await _context.Likes.Where(l => l.PhotoId == photoId && l.MemberId == userId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Photo>> GetTopLikedPhotos(DateTime startDate, DateTime endDate, int topCount)
    {
        return await _context.Likes
            .Where(like => like.LikedOn >= startDate && like.LikedOn <= endDate)
            .GroupBy(like => like.PhotoId)
            .OrderByDescending(g => g.Count())
            .Take(topCount)
            .Select(g => g.Key)
            .Join(_context.Photos, photoId => photoId, photo => photo.Id, (photoId, photo) => photo)
            .ToListAsync();
    }
}
