using Maasgroep.Database.Context;
using Maasgroep.Database.Context.team_d.Models;
using Microsoft.EntityFrameworkCore;
using ms18_applicatie.Interfaces.team_d;

namespace ms18_applicatie.Repository.team_d;

public class TagsRepository : ITagsRepository
{
    private readonly MaasgroepContext _context;

    public TagsRepository(MaasgroepContext context)
    {
        _context = context;
    }

    public async Task<bool> AddTagToAlbumAsync(Guid albumId, Guid tagId)
    {
        var existingLink = await _context.AlbumTags
            .AnyAsync(at => at.AlbumId == albumId && at.TagId == tagId);

        if (existingLink)
        {
            return true;
        }

        var albumTag = new AlbumTag { AlbumId = albumId, TagId = tagId };
        _context.AlbumTags.Add(albumTag);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Tag?> CreateTagAsync(string tagName)
    {
        var tag = new Tag { Id = Guid.NewGuid(), Name = tagName };
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();

        return _context.Tags.FirstOrDefault(t => t.Id == tag.Id);
    }

    public async Task<bool> DeleteTagAsync(Guid tagId)
    {
        var tag = await _context.Tags.FindAsync(tagId);
        if (tag == null)
        {
            return false;
        }

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Album>> GetAlbumsWithTagAsync(Guid tagId)
    {
        var albums = await _context.Albums
            .Where(a => a.AlbumTags.Any(at => at.TagId == tagId))
            .ToListAsync();

        return albums;
    }

    public async Task<Tag?> GetTagByIdAsync(Guid tagId)
    {
        return await _context.Tags.FindAsync(tagId);
    }

    public async Task<bool> RemoveTagFromAlbumAsync(Guid albumId, Guid tagId)
    {
        var albumTag = await _context.AlbumTags
            .FirstOrDefaultAsync(at => at.AlbumId == albumId && at.TagId == tagId);

        if (albumTag == null)
        {
            return false;
        }

        _context.AlbumTags.Remove(albumTag);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Tag?> UpdateTagAsync(Guid tagId, string newName)
    {
        var tag = await _context.Tags.FindAsync(tagId);
        if (tag == null)
        {
            return null;
        }

        tag.Name = newName;
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync();
        return tag;
    }
}

