using e_crime.mvc.Data;
using System.ComponentModel.DataAnnotations;

namespace e_crime.mvc.Models
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
        public ICollection<ApplicationUser> PoliceOfficers { get; set; } = new List<ApplicationUser>();
    }
}
