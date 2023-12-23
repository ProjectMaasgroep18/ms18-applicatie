using Maasgroep.Database.Context.Tables.PhotoAlbum;

namespace Maasgroep.Interfaces;

public interface ILikesRepository
{
    Task AddLike(Like like);
    Task<Like?> GetLike(Guid photoId, long userId);
    Task DeleteLike(Guid likeId);
    Task<IEnumerable<Like>> GetAllLikesForPhoto(Guid photoId);
    Task<IEnumerable<Photo>> GetTopLikedPhotos(DateTime startDate, DateTime endDate, int topCount);
}

