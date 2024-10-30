using e_crime.mvc.Data;
using e_crime.mvc.Enums;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace e_crime.mvc.Models
{
    public class Crime
    {
        [Key]
        public int Id { get; set; }
        public string Location { get; set; } = string.Empty;
        public CrimeType CrimeType { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.UnAssigned;
        public bool IsEdited { get; set; } = false;
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string? AssignedTo { get; set; }
        public ApplicationUser AssignedOfficer { get; set; }

        //public ICollection<UserCrime> UserCrimes { get; set; } = new List<UserCrime>();

    }
}
