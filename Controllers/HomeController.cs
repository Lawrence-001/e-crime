using e_crime.mvc.Data;
using e_crime.mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace e_crime.mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> SubmittedCrimes()
        {
            var crimes = await GetCrimesByUser();

            return View(crimes);
        }

        /// <summary>
        /// Get crimes submitted by a user
        /// </summary>
        /// <returns></returns>
        private async Task<List<Crime>> GetCrimesByUser()
        {
            var user = await _userManager.GetUserAsync(User);
            return await _context.Crimes.Where(u => u.UserId.Equals(user.Id)).ToListAsync();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
