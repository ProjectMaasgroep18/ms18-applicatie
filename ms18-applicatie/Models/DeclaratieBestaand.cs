using System.ComponentModel.DataAnnotations;

namespace ms18_applicatie.Models
{
    public class DeclaratieBestaand
    {
        [Display(Name = "File")]
        public byte[] FormFile { get; set; }

        public string FormFile64 { get; set; }

        [Display(Name = "Opmerking")]
        public string? Note { get; set; }
        [Display(Name = "Bedrag")]
        public decimal? Amount { get; set; }

        public string? AangemaaktDoor { get; set; }
    }
}
