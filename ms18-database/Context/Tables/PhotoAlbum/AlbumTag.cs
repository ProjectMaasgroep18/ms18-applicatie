namespace Maasgroep.Database.Context.Tables.PhotoAlbum;

public class AlbumTag
{
    public Guid AlbumId { get; set; }
    public Album Album { get; set; } = null!;

    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
