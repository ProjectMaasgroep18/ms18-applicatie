namespace Maasgroep.Database.Context.Tables.PhotoAlbum;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public IEnumerable<AlbumTag> AlbumTags { get; set; } = null!;
}

