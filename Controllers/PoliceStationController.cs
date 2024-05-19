using e_crime.Data;
using e_crime.Models;
using e_crime.Services.Interfaces;
using e_crime.ViewModels.PoliceStation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace e_crime.Controllers
{
    public class PoliceStationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IPoliceStationService _policeStationService;

        public PoliceStationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context, IPoliceStationService policeStationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _policeStationService = policeStationService;
        }
        public IActionResult Index()
        {
            var stations = _context.PoliceStations.ToList();
            return View(stations);
        }

        /// <summary>
        /// get users with the role PoliceOfficer
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> Officers()
        {

            //using .net identity
            var role = await _roleManager.FindByNameAsync("PoliceOfficer");
            var userInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            var users = userInRole.Select(x => x.Email).ToList();


            //using LINQ
            //var officers = (from ur in _context.UserRoles
            //                join r in _context.Roles on ur.RoleId equals r.Id
            //                join u in _context.Users on ur.UserId equals u.Id
            //                where r.Name == "PoliceOfficer"
            //                select u.Email).ToList();

            return users;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var policeOfficers = await Officers();
            var selectList = new SelectList(policeOfficers);
            ViewBag.SelectList = selectList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PoliceStationVM policeStationVM)
        {
            var policeOfficers = await Officers();

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
                policeStation.UserId = officer.Id;

                TempData["Success"] = "Police station created successfully";
                await _policeStationService.CreatePoliceStation(policeStation);
                return RedirectToAction("Index");
            }
            return View(policeStationVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var station = await _policeStationService.GetPoliceStationById(id);
            var officers = await Officers();
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

        [HttpPost]
        public async Task<IActionResult> Edit(EditPoliceStationVM model)
        {
            if (ModelState.IsValid)
            {
                var station = await _policeStationService.GetPoliceStationById(model.Id);
                station.Name = model.Name;
                station.Location = model.Location;
                station.County = model.County;
                station.InChargeName = model.InChargeName;

                await _policeStationService.EditPoliceStation(station);
                TempData["Success"] = "Police station updated succefully";
                return RedirectToAction("Index");
            }
            return View(model);
        }

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
    }
}
