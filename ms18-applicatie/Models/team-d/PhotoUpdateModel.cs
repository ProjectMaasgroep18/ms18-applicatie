using System.ComponentModel.DataAnnotations;

namespace ms18_applicatie.Models.team_d;
public class PhotoUpdateModel
{
    public string? Title { get; set; }
    [Required]
    public string ContentType { get; set; } = null!;
    public DateTime? TakenOn { get; set; }
    public string? Location { get; set; }
    [Required]
    public Guid AlbumLocationId { get; set; }
    [Required]
    public bool NeedsApproval { get; set; }
}
