using e_crime.mvc.Data;
using e_crime.mvc.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace e_crime.mvc.Controllers
{
    //[Authorize]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// check if email is already in database
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailTaken(string email)
        {
            var userEmail = await _userManager.FindByEmailAsync(email);
            if (userEmail != null)
            {
                return Json($"Email {email} is already taken");
            }
            else
            {
                return Json(true);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegistrationVM registrationVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    FirstName = registrationVM.FName,
                    LastName = registrationVM.LName,
                    Address = registrationVM.Address,
                    Gender = registrationVM.Gender,
                    Email = registrationVM.Email,
                    MobileNumber = registrationVM.MobileNumber,
                };

                user.UserName = registrationVM.Email;
                var result = await _userManager.CreateAsync(user, registrationVM.Password);

                await _userManager.AddToRoleAsync(user, "User");

                if (result.Succeeded)
                {
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Super Admin"))
                    {
                        TempData["Message"] = "User created successfully";
                        //user.CreatedBy = User.
                        return RedirectToAction("AllUsers", "Administration");
                    }

                    TempData["Successful"] = " Account created successfully, proceed to login";
                    return RedirectToAction("Login");
                }

                foreach (var error in result.Errors)
                {

                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(registrationVM);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, false, false);
                if (user.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid credentials, please try again");
                }
            }
            return View(loginVM);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        await _signInManager.RefreshSignInAsync(user);
                        TempData["Successful"] = "Password changed successfully, proceed to login";
                        return RedirectToAction("Login", "Auth");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        return View();
                    }
                }
            }
            return View(model);
        }
    }
}
