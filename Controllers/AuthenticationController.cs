using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaApp.Application.Common.Interfaces;
using VillaApp.Application.Common.Utilities;
using VillaApp.Domains.Entities;
using VillaApp.Web.ViewModels;

namespace VillaApp.Controllers
{
    public class AuthenticationController : Controller
    {
        internal readonly IUnitOfWork _unitOfWork;
        internal readonly UserManager<ApplicationUser> _userManager;
        internal readonly SignInManager<ApplicationUser> _signInManager;
        internal readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork    = unitOfWork;
            _userManager   = userManager;
            _signInManager = signInManager;
            _roleManager   = roleManager;
        }
        public IActionResult Login(String? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var obj = new LoginVM
            {
                RedirectURL = returnUrl
            };
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM _params)
        {
            var loginStatus = await _signInManager
                .PasswordSignInAsync(_params.Email!, _params.Password!,_params.RememberMe, false);
            if (loginStatus.Succeeded)
            {
                if (!string.IsNullOrEmpty(_params.RedirectURL))
                {
                    return LocalRedirect(_params.RedirectURL!);
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
            return View(_params);
        }

        public IActionResult SignUp()
        {
            if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).Wait();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Customer)).Wait();
            }

            RegisterVM obj = new()
            {
                RolesList = [.. _roleManager.Roles
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Name
                    })]
            };
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterVM _params)
        {
            ApplicationUser user = new()
            {
                Name            = _params.Name,
                Email           = _params.Email,
                PhoneNumber     = _params.PhoneNumber,
                NormalizedEmail = _params.Email!.ToUpper(),
                EmailConfirmed  = true,
                UserName        = _params.Email,
                CreatedAt       = DateTime.Now
            };
            var result = await _userManager.CreateAsync(user, _params.Password!); // Helps to create the User
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(_params.ApplicationRoles))
                {
                    await _userManager.AddToRoleAsync(user, _params.ApplicationRoles);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, StaticDetails.Role_Customer);
                }
                await _signInManager.SignInAsync(user, false); //Used to signin and persistence is for cookies
                if (!string.IsNullOrEmpty(_params.RedirectURL))
                {
                    return LocalRedirect(_params.RedirectURL!); // Redirects to the own domain not any other malicious website
                }
                else
                    return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description); // Will display all errors in Model Summary because its Model Only
            }
            // We will now save role based on the input params
            _params.RolesList =
            [
                .. _roleManager.Roles
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Name
                    })
            ];
            return View(_params);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}