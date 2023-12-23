using Maasgroep.Database.Context.Tables.PhotoAlbum;

namespace Maasgroep.Interfaces;

public interface IPhotoRepository
{
    Task AddPhoto(Photo photo);
    Task<Photo?> GetPhotoById(Guid photoId);
    Task UpdatePhoto(Photo photo);
    Task DeletePhoto(Guid photoId);
    Task<(IEnumerable<Photo> Photos, int TotalCount)> GetPhotosByAlbumId(Guid albumId, int pageNumber, int pageSize);

}

