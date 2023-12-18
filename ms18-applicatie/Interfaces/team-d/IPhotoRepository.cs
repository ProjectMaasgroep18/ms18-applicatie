using Maasgroep.Database.Context.team_d.Models;

namespace ms18_applicatie.Interfaces.team_d;

public interface IPhotoRepository
{
    Task AddPhoto(Photo photo);
    Task<Photo?> GetPhotoById(Guid photoId);
    Task UpdatePhoto(Photo photo);
    Task DeletePhoto(Guid photoId);
    Task<(IEnumerable<Photo> Photos, int TotalCount)> GetPhotosByAlbumId(Guid albumId, int pageNumber, int pageSize);

}

