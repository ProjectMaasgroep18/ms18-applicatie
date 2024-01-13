using Maasgroep.Database.Context.Tables.PhotoAlbum;
using ms18_applicatie.Models.team_d;

namespace ms18_applicatie.Interfaces;

public interface IAlbumRepository
{
    Task<bool> AlbumExists(string albumName, Guid? parentAlbumId);
    Task<Guid?> AddAlbum(AlbumCreateModel album);
    Task<AlbumViewModel?> GetAlbumViewModelById(Guid id);
    Task<Album?> GetAlbumById(Guid id);
    Task<List<AlbumViewModel>> GetAllAlbums();
    Task UpdateAlbum(Album album);
    Task<bool> DeleteAlbum(Guid albumId);
}
