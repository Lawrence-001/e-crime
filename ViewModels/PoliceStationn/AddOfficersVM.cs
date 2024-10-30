namespace e_crime.mvc.ViewModels.PoliceStationn
{
    public class AddOfficersVM
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsSelected { get; set; }

        //public List<ApplicationUser> Officers { get; set; } = new List<ApplicationUser>();
    }
}