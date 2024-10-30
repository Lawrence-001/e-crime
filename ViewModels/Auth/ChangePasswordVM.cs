using System.ComponentModel.DataAnnotations;

namespace e_crime.mvc.ViewModels.Auth
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "Current Password cannot be empty")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "New Password cannot be empty")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm New Password cannot be empty")]
        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The New Password and confirm password do not match")]
        public string ConfirmNewPassword { get; set; }
    }
}
