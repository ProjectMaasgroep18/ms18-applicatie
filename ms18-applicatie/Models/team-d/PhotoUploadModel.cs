using System.ComponentModel.DataAnnotations;

namespace ms18_applicatie.Models.team_d
{
    public class PhotoUploadModel
    {
        public string? Title { get; set; }
        [Required]
        public string ImageData { get; set; } = null!;
        [Required]
        public string ContentType { get; set; } = null!;
        public string? Location { get; set; }
        public DateTime? TakenOn { get; set; }
        [Required]
        public Guid AlbumId { get; set; }
    }

}
