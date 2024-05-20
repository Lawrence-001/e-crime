using e_crime.Data;

namespace e_crime.Models
{
    public class UserCrime
    {
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int CrimeId { get; set; }
        public Crime Crime { get; set; }
    }
}
