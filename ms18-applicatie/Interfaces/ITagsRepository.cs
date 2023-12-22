using Maasgroep.Database.Context.Tables.PhotoAlbum;

namespace ms18_applicatie.Interfaces;

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

