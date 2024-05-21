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
        public async Task<IActionResult> Index()
        {
            var stations = await _policeStationService.GetPoliceStations();
            return View(stations);
        }

        /// <summary>
        /// get users with the role PoliceOfficer
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
        public async Task<IActionResult> Create()
        {
            var policeOfficers = await GetOfficersEmail();
            var selectList = new SelectList(policeOfficers);
            ViewBag.SelectList = selectList;
            return View();
        }

        [HttpPost]
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
