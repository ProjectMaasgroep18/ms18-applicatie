using System.ComponentModel.DataAnnotations;

namespace Ms18.Application.Models.TeamC
{
    public class DeclaratieViewModel
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }

        [Display(Name = "Opmerking")]
        public string? Note { get; set; }
        [Display(Name = "Bedrag")]
        public decimal? Amount { get; set; }
    }
}
