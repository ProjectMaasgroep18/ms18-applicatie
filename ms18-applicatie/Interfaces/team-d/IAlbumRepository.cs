using Maasgroep.Database.Context.team_d.Models;

namespace ms18_applicatie.Interfaces.team_d;

public interface IAlbumRepository
{
    Task<bool> AlbumExists(string albumName, Guid? parentAlbumId);
    Task AddAlbum(Album album);
    Task<Album?> GetAlbumById(Guid id);
    Task<IEnumerable<Album>> GetAllAlbums();
    Task UpdateAlbum(Album album);
}


