using e_crime.Enums;
using Microsoft.AspNetCore.Identity;

namespace e_crime.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }
}
