using HomeOwners_CasaMira_Web.Data;
using HomeOwners_CasaMira_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HomeOwners_CasaMira_Web.Controllers
{
    [Authorize(Roles = "Admin")] // Only admins can access
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            // Total number of facilities
            var totalFacilities = _context.Facilities.Count();

            // Group facility reservations by facility name
            var reservedFacilities = _context.FacilityReservations
                .GroupBy(fr => fr.Facility.Name)
                .Select(g => new
                {
                    FacilityName = g.Key,
                    ReservationCount = g.Count()
                })
                .ToList();

            // Pass to view
            ViewBag.TotalFacilities = totalFacilities;
            ViewBag.ReservedFacilities = reservedFacilities;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnnouncement(Announcement model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.UtcNow;
                model.DatePosted = DateTime.UtcNow;

                _context.Announcements.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Announcement posted successfully!";
                return RedirectToAction("Dashboard"); // Redirects after posting
            }

            return View("Dashboard", model);
        }
    }
}
