namespace ms18_applicatie.Models.team_d;
public class PhotoUpdateModel
{
    public string? Title { get; set; } = null!;
    public DateTime? TakenOn { get; set; }
    public string? Location { get; set; }
    public Guid AlbumLocationId { get; set; }
}
