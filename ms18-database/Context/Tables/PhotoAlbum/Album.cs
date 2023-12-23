namespace Maasgroep.Database.Context.Tables.PhotoAlbum;

public class Album
{
    public Album()
    {
    }

    public Album(string name)
    {
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int? Year { get; set; }

    public Guid? ParentAlbumId { get; set; }
    public Album? ParentAlbum { get; set; }

    public Guid? CoverPhotoId { get; set; }
    public Photo? CoverPhoto { get; set; }

    public IEnumerable<Album> ChildAlbums { get; set; } = null!;
    public IEnumerable<Photo> Photos { get; set; } = null!;
}

