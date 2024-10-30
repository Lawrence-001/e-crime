using System.ComponentModel.DataAnnotations;

namespace e_crime.mvc.ViewModels.Administration
{
    public class CreateRoleVM
    {
        [Required(ErrorMessage = "Name cannot be empty")]
        [Display(Name = "Role Name")]
        public string Name { get; set; } = string.Empty;
    }
}