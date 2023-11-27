namespace Ms18.Database.Models.TeamD.PhotoAlbum;
public record PhotoTag
{
    public Guid PhotoId { get; set; }
    public Photo Photo { get; set; } = null!;

    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}


