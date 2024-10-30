namespace e_crime.mvc.ViewModels.Administration
{
    public class UserRoleVM
    {
        public string UserId { get; set; } = string.Empty;
        //public string FName { get; set; }
        //public string LName { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
