namespace ms18_applicatie.Models.team_d;

public class PhotoViewModel
{
    public Guid Id { get; set; }
    public long UploaderId { get; set; }
    public DateTime UploadDate { get; set; }
    public string? Title { get; set; }
    public string ImageBase64 { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public DateTime? TakenOn { get; set; }
    public string? Location { get; set; }
    public Guid AlbumLocationId { get; set; }
    public int LikesCount { get; set; }
}
