using System.ComponentModel.DataAnnotations;

namespace e_crime.mvc.ViewModels.ContactUs
{
    public class ContactUsVM
    {
        [Required(ErrorMessage = "Subject cannot be empty")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Message cannot be empty")]
        public string Message { get; set; }
    }
}
