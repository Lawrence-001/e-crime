using System.ComponentModel.DataAnnotations;

namespace e_crime.mvc.Models
{
    public class ContactUs
    {
        [Key]
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
