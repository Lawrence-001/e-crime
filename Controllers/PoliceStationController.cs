using e_crime.mvc.Data;
using e_crime.mvc.Models;
using e_crime.mvc.Services.Interfaces;
using e_crime.mvc.ViewModels.PoliceStationn;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace e_crime.mvc.Controllers
{
    //[Authorize]
    public class PoliceStationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IPoliceStationService _policeStationService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public PoliceStationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context, IPoliceStationService policeStationService, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _policeStationService = policeStationService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var stations = await _policeStationService.GetPoliceStations();
            return View(stations);
        }

        /// <summary>
        /// get users with the role Officer
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetOfficersEmail()
        {
            //using .net identity
            var role = await _roleManager.FindByNameAsync("Officer");
            var userInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            var users = userInRole.Select(x => x.Email).ToList();

            //var officerIds = await GetOfficersIds(GetOfficersEmail);

            //using LINQ
            //var officers = (from ur in _context.UserRoles
            //                join r in _context.Roles on ur.RoleId equals r.Id
            //                join u in _context.Users on ur.UserId equals u.Id
            //                where r.Name == "PoliceOfficer"
            //                select u.Email).ToList();

            return users;
        }

        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Create()
        {
            var policeOfficers = await GetOfficersEmail();
            var selectList = new SelectList(policeOfficers);
            ViewBag.SelectList = selectList;
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Create(PoliceStationVM policeStationVM)
        {
            var policeOfficers = await GetOfficersEmail();

            var inChargeOfficer = policeOfficers.FirstOrDefault(x => x.Equals(policeStationVM.InchargeEmail));
            if (inChargeOfficer == null)
            {
                ModelState.AddModelError("", "Police Officer not found");
                return View(policeStationVM);
            }

            var officer = await _userManager.FindByEmailAsync(policeStationVM.InchargeEmail);

            if (ModelState.IsValid)
            {
                PoliceStation policeStation = new PoliceStation()
                {
                    Name = policeStationVM.Name,
                    InchargeEmail = officer.Email,
                    InChargeName = officer.FirstName + " " + officer.LastName,
                    Location = policeStationVM.Location,
                    County = policeStationVM.County,
                };

                await _policeStationService.CreatePoliceStation(policeStation);

                officer.PoliceStationId = policeStation.Id;
                _context.SaveChanges();

                TempData["Success"] = "Police station created successfully";
                return RedirectToAction("Index");
            }
            return View(policeStationVM);
        }

        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var station = await _policeStationService.GetPoliceStationById(id);
            var officers = await GetOfficersEmail();
            var selectList = new SelectList(officers);
            ViewBag.SelectList = selectList;

            if (station != null)
            {
                EditPoliceStationVM editStationVM = new EditPoliceStationVM()
                {
                    Name = station.Name,
                    Location = station.Location,
                    County = station.County,
                    InchargeEmail = station.InchargeEmail,
                    InChargeName = station.InChargeName
                };
                return View(editStationVM);
            }

            ModelState.AddModelError("", "Police station not found");
            return RedirectToAction("Index");
        }

        //public async Task<List<string>> GetOfficersIds(Func<Task<List<string>>> getOfficersEmails)
        //{
        //    var emails = await getOfficersEmails(); // Invoke the function to get emails
        //    var officerIds = new List<string>();
        //    foreach (var email in emails)
        //    {
        //        var user = await _userManager.FindByEmailAsync(email);
        //        if (user != null)
        //        {
        //            officerIds.Add(user.Id);
        //        }
        //    }
        //    return officerIds;
        //}
        //public async Task<List<string>> GetOfficersIds()
        //{
        //    var role = await _roleManager.FindByNameAsync("Officer");
        //    var userInRole = await _userManager.GetUsersInRoleAsync(role.Name);
        //    var users = userInRole.Select(x => x.Id).ToList();

        //    return users;
        //}

        public async Task<List<string>> GetOfficersIds()
        {
            var emails = await GetOfficersEmail();
            var ids = new List<string>();
            foreach (var email in emails)
            {
                var user = await _userManager.FindByEmailAsync(email);
                ids.Add(user.Id);
            }
            return ids;
        }

        [HttpPost]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Edit(EditPoliceStationVM model)
        {
            var officers = await GetOfficersIds();

            //var currentInCharge = await _userManager.FindByIdAsync(officerId);
            if (ModelState.IsValid)
            {
                var station = await _policeStationService.GetPoliceStationById(model.Id);
                station.Name = model.Name;
                station.Location = model.Location;
                station.County = model.County;
                //station.InChargeName = currentInCharge.FirstName + " " + currentInCharge.LastName;
                station.InchargeEmail = model.InchargeEmail;

                _context.SaveChanges();
                await _policeStationService.EditPoliceStation(station);
                TempData["Success"] = "Police station updated succefully";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var stationToDelete = await _policeStationService.GetPoliceStationById(id);
            if (stationToDelete != null)
            {
                _policeStationService.DeletePoliceStation(id);
            }
            TempData["Message"] = "Police station deleted succefully";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StationOfficers(int id)
        {
            var station = await _policeStationService.GetPoliceStationById(id);
            if (station != null)
            {
                ViewBag.StationName = station.Name;
                var officers = await _policeStationService.StationOfficers(station.Location);

                return View(officers);
            }
            return View("NotFound");
        }

        [HttpGet]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> AddOfficer()
        {
            var stations = await _policeStationService.GetPoliceStations();
            ViewBag.Stations = stations.Select(station => new SelectListItem
            {
                Value = station.Id.ToString(),
                Text = station.Name,
            });
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> AddOfficer(CreateOfficerVM createOfficerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser officer = new ApplicationUser()
                {
                    FirstName = createOfficerVM.FName,
                    LastName = createOfficerVM.FName,
                    Address = createOfficerVM.Address,
                    Gender = createOfficerVM.Gender,
                    MobileNumber = createOfficerVM.MobileNumber,
                    Email = createOfficerVM.Email,
                    PoliceStationId = createOfficerVM.SelectedStation,
                };
                officer.UserName = createOfficerVM.Email;
                var result = await _userManager.CreateAsync(officer, createOfficerVM.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(officer, "Officer");
                    TempData["Success"] = "Officer created succefully";
                    return RedirectToAction("index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            var stations = await _policeStationService.GetPoliceStations();
            ViewBag.Stations = stations.Select(station => new SelectListItem
            {
                Value = station.Id.ToString(),
                Text = station.Name,
            });
            return View(createOfficerVM);
        }

        [HttpGet]
        public async Task<IActionResult> AddOfficers(int id)
        {
            var station = await _policeStationService.GetPoliceStationById(id);
            if (station != null)
            {
                var officersList = new List<AddOfficersVM>();
            }
            return View();
        }
    }
}