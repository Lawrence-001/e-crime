using e_crime.Data;
using System.ComponentModel.DataAnnotations;

namespace e_crime.Models
{
    public class PoliceStation
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string InChargeName { get; set; } = string.Empty;
        public string InchargeEmail { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string County { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; }
    }
}
