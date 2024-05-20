using e_crime.Enums;
using e_crime.Models;
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
        public int? PoliceStationId { get; set; }  //not all users are related to a police station
        public PoliceStation PoliceStation { get; set; }
        public ICollection<UserCrime> UserCrimes { get; set; } = new List<UserCrime>();
    }
}
