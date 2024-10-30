using e_crime.mvc.Enums;
using System.ComponentModel.DataAnnotations;

namespace e_crime.mvc.ViewModels.Crime
{
    public class EditCrimeVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Location cannot be empty")]
        public string Location { get; set; } = string.Empty;
        [Required(ErrorMessage = "Type of crime cannot be empty")]
        public CrimeType CrimeType { get; set; }
        [Required(ErrorMessage = "Date of crime cannot be empty")]
        [Display(Name = "Date of crime")]
        public DateTime DateTime { get; set; }
        [Required(ErrorMessage = "Crime description cannot be empty")]
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.UnAssigned;
        public bool IsEdited { get; set; } = false;
    }
}