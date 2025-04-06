using HomeOwners_CasaMira_Web.Models;
using HomeOwners_CasaMira_Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeOwners_CasaMira_Web.Controllers
{
    public class AccountController : Controller
    {

        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;

        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View(model);
                }

                var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);
                if (!passwordValid)
                {
                    ModelState.AddModelError("", "Invalid password.");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (roles.Contains("Staff"))
                    {
                        return RedirectToAction("Dashboard", "Staff");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home"); // For normal users
                    }
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "User account locked out.");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect.");
                }
            }
            return View(model);
        }



        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                    Address = model.Address,
                    DateOfBirth = model.DateOfBirth,
                    Role = model.Role 
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign the selected role to the user
                    await userManager.AddToRoleAsync(user, model.Role);

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }
        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username)) {
                return RedirectToAction("VerifyEmail", "Account");
        
            }
            return View(new ChangePasswordViewModel{ Email = username });
        }

        public async Task<IActionResult> Logout()

        { 
          await signInManager.SignOutAsync();
          return RedirectToAction("Index", "Home");
        }
   
       

    }
}
