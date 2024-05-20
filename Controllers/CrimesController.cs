using e_crime.Data;
using e_crime.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace e_crime.Controllers
{
    public class CrimesController : Controller
    {
        private readonly ICrimeService _crime;
        private readonly UserManager<ApplicationUser> _userManager;

        public CrimesController(ICrimeService crime, UserManager<ApplicationUser> userManager)
        {
            _crime = crime;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
