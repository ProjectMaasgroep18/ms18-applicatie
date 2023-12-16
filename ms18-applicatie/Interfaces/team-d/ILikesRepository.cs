using Maasgroep.Database.Context.team_d.Models;

namespace ms18_applicatie.Interfaces.team_d;

public interface ILikesRepository
{
    Task AddLike(Like like);
    Task<Like?> GetLike(Guid photoId, long userId);
    Task DeleteLike(Guid likeId);
    Task<IEnumerable<Like>> GetAllLikesForPhoto(Guid photoId);
    Task<IEnumerable<Photo>> GetTopLikedPhotos(DateTime startDate, DateTime endDate, int topCount);
}

