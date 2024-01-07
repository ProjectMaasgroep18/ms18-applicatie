using System.ComponentModel.DataAnnotations;

namespace ms18_applicatie.Models.team_d;

public class PhotoViewModel
{
    [Required]
    public Guid Id { get; set; }
    public long? UploaderId { get; set; }
    [Required]
    public DateTime UploadDate { get; set; }
    public string? Title { get; set; }
    [Required]
    public string ImageBase64 { get; set; } = null!;
    [Required]
    public string ContentType { get; set; } = null!;
    public DateTime? TakenOn { get; set; }
    public string? Location { get; set; }
    public Guid? AlbumLocationId { get; set; }
    [Required]
    public int LikesCount { get; set; }
    [Required]
    public bool NeedsApproval { get; set; }
}
