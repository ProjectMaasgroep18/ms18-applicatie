using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ms18_applicatie.Models
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
