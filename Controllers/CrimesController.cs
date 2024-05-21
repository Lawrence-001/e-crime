using e_crime.Data;
using e_crime.Models;
using e_crime.Services.Interfaces;
using e_crime.ViewModels.Crime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace e_crime.Controllers
{
    [Authorize]
    public class CrimesController : Controller
    {
        private readonly ICrimeService _crimeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CrimesController(ICrimeService crimeService, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _crimeService = crimeService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var crimes = await _crimeService.GetAllCrimes();
            return View(crimes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCrimeVM createCrimeVM)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                Crime newCrime = new Crime()
                {
                    Location = createCrimeVM.Location,
                    CrimeType = createCrimeVM.CrimeType,
                    DateTime = createCrimeVM.DateTime,
                    Description = createCrimeVM.Description,
                    Status = createCrimeVM.Status,
                    IsEdited = createCrimeVM.IsEdited,
                    UserId = user.Id,

                };
                await _crimeService.CreateCrime(newCrime);

                if (_signInManager.IsSignedIn(User) && User.IsInRole("User")
                    || User.IsInRole("Officer") || User.IsInRole("Super Admin"))
                {
                    TempData["success"] = "Cime submitted succefully";
                    return RedirectToAction("SubmittedCrimes", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(createCrimeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var crime = await _crimeService.GetCrimeById(id);
            if (crime != null)
            {
                EditCrimeVM editCrimeVM = new EditCrimeVM()
                {
                    Location = crime.Location,
                    CrimeType = crime.CrimeType,
                    DateTime = crime.DateTime,
                    Description = crime.Description,
                    Status = crime.Status,
                    IsEdited = crime.IsEdited,
                };
                return View(editCrimeVM);
            }
            ModelState.AddModelError("", $"Crime with id {crime.Id} not found");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCrimeVM editCrimeVM)
        {
            if (ModelState.IsValid)
            {
                var crimeToEdit = await _crimeService.GetCrimeById(editCrimeVM.Id);
                if (crimeToEdit != null)
                {
                    crimeToEdit.Location = editCrimeVM.Location;
                    crimeToEdit.CrimeType = editCrimeVM.CrimeType;
                    crimeToEdit.DateTime = editCrimeVM.DateTime;
                    crimeToEdit.Description = editCrimeVM.Description;
                    crimeToEdit.Status = editCrimeVM.Status;
                    crimeToEdit.IsEdited = true;
                }
                await _crimeService.EditCrime(crimeToEdit);
                TempData["message"] = "Crime updated succefully";
                return RedirectToAction("SubmittedCrimes", "Home");
            }
            return View(editCrimeVM);

        }
    }
}
