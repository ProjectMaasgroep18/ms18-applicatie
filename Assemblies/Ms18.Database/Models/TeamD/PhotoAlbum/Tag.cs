namespace Ms18.Database.Models.TeamD.PhotoAlbum;
public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public IEnumerable<PhotoTag> PhotoTags { get; set; } = null!;
}
