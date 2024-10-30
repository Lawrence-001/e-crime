using e_crime.mvc.Data;
using e_crime.mvc.Models;
using e_crime.mvc.Services.Interfaces;
using e_crime.mvc.ViewModels.Crime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace e_crime.mvc.Controllers
{
    //[Authorize]
    public class CrimesController : Controller
    {
        private readonly ICrimeService _crimeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public CrimesController(ICrimeService crimeService, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _crimeService = crimeService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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
                    TempData["success"] = "Crime submitted succefully";
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

        [Authorize(Roles = "Super Admin")]
        public IActionResult Delete(int id)
        {
            var crime = _crimeService.GetCrimeById(id);
            if (crime != null)
            {
                _crimeService.DeleteCrime(id);
            }
            return View();
        }

        //public async Task<IActionResult> GetCrimesByOfficerLocation()
        //{
        //    if (_signInManager.IsSignedIn(User) && User.IsInRole("Officer"))
        //    {
        //        var officer = await _userManager.GetUserAsync(User);
        //        if (officer != null)
        //        {
        //            var officerLocation = officer.Address;

        //            var crimesInOfficerLocation = await _context.Crimes
        //                                                        .Where(l => l.Location.Equals(officerLocation))
        //                                                        .ToListAsync();

        //            if (crimesInOfficerLocation.Any())
        //            {
        //                return Ok(crimesInOfficerLocation);
        //            }
        //            else
        //            {
        //                return NotFound("No crimes found for the officer's location.");
        //            }
        //        }
        //        return Unauthorized("Officer not found.");
        //    }
        //    return Unauthorized("User is not in the Officer role.");
        //}

        [Authorize(Roles = "Officer")]
        [Authorize(Roles = "InCharge")]
        public async Task<IActionResult> CrimeByPoliceStation()
        {
            if (_signInManager.IsSignedIn(User) && User.IsInRole("Officer"))
            {
                var officer = await _userManager.GetUserAsync(User);

                if (officer == null)
                {
                    return Unauthorized("Officer not found.");
                }
                try
                {
                    var crimes = await _crimeService.GetCrimesByOfficerLocation(officer.Email, officer.Address);
                    if (!crimes.Any())
                    {
                        return NotFound("No crimes found for the officer's location.");
                    }

                    return View(crimes);
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }
            return Unauthorized("User is not in the Officer role.");
        }

        public async Task<IActionResult> AssignCrimeToOfficer()
        {
            return View();
        }
    }
}
