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
                // Check if the user exists
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View(model);
                }

                // Verify the password
                bool passwordValid = await userManager.CheckPasswordAsync(user, model.Password);
                if (!passwordValid)
                {
                    ModelState.AddModelError("", "Invalid password.");
                    return View(model);
                }

                // Sign in the user
                var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // 🔹 Check if user is an Admin
                    if (await userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Dashboard", "Admin"); // Redirect to Admin Panel
                    }

                    return RedirectToAction("Index", "Home"); // Normal users go to Home
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "User account locked out.");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "User is not allowed to sign in.");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect");
                }
            }
            return View(model);
        }


        public IActionResult Register()
        {
            return View();
        }

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
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
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
