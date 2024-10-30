using e_crime.mvc.Data;
using e_crime.mvc.ViewModels.ContactUs;
using Microsoft.AspNetCore.Mvc;

namespace e_crime.mvc.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactUsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactUsVM model)
        {
            if (ModelState.IsValid)
            {
                var info = new Models.ContactUs()
                {
                    Id= Guid.NewGuid().ToString(),
                    Subject = model.Subject,
                    Message = model.Message,
                };

                await _context.ContactUs.AddAsync(info);
                _context.SaveChanges();
                TempData["Successful"] = "Message send successfully";

                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
