using Maasgroep.Database.Context.team_d.Models;

namespace ms18_applicatie.Interfaces.team_d;

public interface IAlbumRepository
{
    Task<bool> AlbumExists(Guid albumId);
    Task<Album> CreateAlbum(Album album);
}


