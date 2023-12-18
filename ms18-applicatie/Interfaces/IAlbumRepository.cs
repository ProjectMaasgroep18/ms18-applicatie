using Maasgroep.Database.Context.Tables.PhotoAlbum;

namespace ms18_applicatie.Interfaces;

public interface IAlbumRepository
{
    Task<bool> AlbumExists(string albumName, Guid? parentAlbumId);
    Task AddAlbum(Album album);
    Task<Album?> GetAlbumById(Guid id);
    Task<IEnumerable<Album>> GetAllAlbums();
    Task UpdateAlbum(Album album);
    Task DeleteAlbum(Guid albumId);
}


