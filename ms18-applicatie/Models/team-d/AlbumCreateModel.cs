using System.ComponentModel.DataAnnotations;

namespace ms18_applicatie.Models.team_d;

public class AlbumCreateModel
{
    [Required]
    public string Name { get; set; } = null!;
    public Guid? ParentAlbumId { get; set; }
    public int? Year { get; set; }

}
