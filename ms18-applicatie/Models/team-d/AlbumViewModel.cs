using System.ComponentModel.DataAnnotations;

namespace ms18_applicatie.Models.team_d;
public class AlbumViewModel
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public int? Year { get; set; }
    public Guid? CoverPhotoId { get; set; }
    public Guid? ParentAlbumId { get; set; }
    public IEnumerable<ChildAlbumViewModel>? ChildAlbums { get; set; }
}

