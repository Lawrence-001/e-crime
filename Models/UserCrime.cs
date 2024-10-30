using e_crime.mvc.Data;

namespace e_crime.mvc.Models
{
    public class UserCrime
    {
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int CrimeId { get; set; }
        public Crime Crime { get; set; }
    }
}
