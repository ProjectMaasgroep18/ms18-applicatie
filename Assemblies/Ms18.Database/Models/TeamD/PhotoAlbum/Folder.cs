namespace Ms18.Database.Models.TeamD.PhotoAlbum;
public record Folder
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public Guid? ParentFolderId { get; set; }
    public Folder? ParentFolder { get; set; }

    public IEnumerable<Folder> ChildFolders { get; set; } = null!;
    public IEnumerable<Photo> Photos { get; set; } = null!;
}
