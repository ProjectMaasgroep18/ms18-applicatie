namespace Maasgroep.Database.Context.team_d.Models;

public class Album
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public Guid? ParentAlbumId { get; set; }
    public Album? ParentAlbum { get; set; }

    public IEnumerable<Album> ChildAlbums { get; set; } = null!;
    public IEnumerable<Photo> Photos { get; set; } = null!;
    public IEnumerable<AlbumTag> AlbumTags { get; set; } = null!;
}

