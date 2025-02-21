using Microsoft.AspNetCore.Mvc;

namespace HomeOwners_CasaMira_Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }


        public IActionResult Register()
        {
            return View();
        }
    }
}
