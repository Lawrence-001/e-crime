using e_crime.Enums;
using System.ComponentModel.DataAnnotations;

namespace e_crime.Models
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

    }
}
