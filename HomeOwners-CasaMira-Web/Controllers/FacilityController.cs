using HomeOwners_CasaMira_Web.Data;
using HomeOwners_CasaMira_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeOwners_CasaMira_Web.Controllers
{
    [Authorize(Roles = "Admin")] // Restrict access to admins only
    public class FacilityController : Controller
    {
        private readonly AppDbContext _context;

        public FacilityController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Facility/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Facility/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,IsAvailable,ImageUrl")] Facility facility)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facility);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(facility);
        }

        // GET: Facility
        public async Task<IActionResult> Index()
        {
            var facilities = await _context.Facilities.ToListAsync();
            return View(facilities);
        }
    }

}
