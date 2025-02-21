using Microsoft.AspNetCore.Mvc;

namespace HomeOwners_CasaMira_Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
