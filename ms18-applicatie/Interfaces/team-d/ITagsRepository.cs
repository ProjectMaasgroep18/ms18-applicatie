using Maasgroep.Database.Context.team_d.Models;

namespace ms18_applicatie.Interfaces.team_d;

public interface ITagsRepository
{
    Task<Tag?> CreateTagAsync(string tagName);
    Task<Tag?> GetTagByIdAsync(Guid tagId);
    Task<bool> DeleteTagAsync(Guid tagId);
    Task<Tag?> UpdateTagAsync(Guid tagId, string newName);
    Task<bool> AddTagToAlbumAsync(Guid albumId, Guid tagId);
    Task<bool> RemoveTagFromAlbumAsync(Guid albumId, Guid tagId);
    Task<IEnumerable<Album?>> GetAlbumsWithTagAsync(Guid tagId);
}

