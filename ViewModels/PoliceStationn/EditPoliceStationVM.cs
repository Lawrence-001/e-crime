using System.ComponentModel.DataAnnotations;

namespace e_crime.mvc.ViewModels.PoliceStationn
{
    public class EditPoliceStationVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name cannot be empty")]
        [Display(Name = "Station Name")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "InCharge Name cannot be empty")]
        [Display(Name = "Incharge Name")]
        public string? InChargeName { get; set; }
        [Required(ErrorMessage = "Location cannot be empty")]
        public string Location { get; set; } = string.Empty;
        [Required(ErrorMessage = "County cannot be null")]
        public string County { get; set; } = string.Empty;
        [Required(ErrorMessage = "InCharge Email cannot be empty")]
        [Display(Name = "Station InCharge")]
        public string InchargeEmail { get; set; } = string.Empty;
        //public string? InchargeId { get; set; }
        //public List<ApplicationUser>? applicationUsers { get; set; } = new List<ApplicationUser>();
    }
}
