using System.ComponentModel.DataAnnotations;

namespace ms18_applicatie.Models.team_d;

public class AlbumUpdateModel
{
    [Required]
    public string Name { get; set; } = null!;
    public int? Year { get; set; }
    public Guid? ParentAlbumId { get; set; }
    public Guid? CoverPhotoId { get; set; }
}

