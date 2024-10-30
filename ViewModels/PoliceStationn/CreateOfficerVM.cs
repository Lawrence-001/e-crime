using e_crime.mvc.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace e_crime.mvc.ViewModels.PoliceStationn
{
    public class CreateOfficerVM
    {
        [Required(ErrorMessage = "First Name cannot be empty")]
        [Display(Name = "First Name")]
        public string FName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last Name cannot be empty")]
        [Display(Name = "Last Name")]
        public string LName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Address cannot be empty")]
        public string Address { get; set; } = string.Empty;
        [Required(ErrorMessage = "Gender cannot be empty")]
        public Gender Gender { get; set; }
        [Required(ErrorMessage = "Email cannot be empty")]
        [EmailAddress]
        [Remote(action: "IsEmailTaken", controller: "Auth")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mobile number cannot be empty")]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password cannot be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Confirm Password cannot be empty")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Display(Name ="Selected Station")]
        public int SelectedStation { get; set; }
    }
}