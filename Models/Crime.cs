using e_crime.Enums;
using System.ComponentModel.DataAnnotations;

namespace e_crime.Models
{
    public class Crime
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Location of Crime")]
        public string Location { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Type of Crime")]
        public CrimeType CrimeType { get; set; }
        [Required]
        [Display(Name = "Date & Time")]
        public DateTime DateTime { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.UnAssigned;
    }
}
