using System.ComponentModel.DataAnnotations;

namespace ms18_applicatie.Models.team_d;

public class ChildAlbumViewModel
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public Guid? CoverPhotoId { get; set; }
}

