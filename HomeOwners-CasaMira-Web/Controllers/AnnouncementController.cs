using HomeOwners_CasaMira_Web.Data;
using HomeOwners_CasaMira_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace HomeOwners_CasaMira_Web.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly AppDbContext _context;

        public AnnouncementController(AppDbContext context)
        {
            _context = context;
        }

        // Show list of announcements
        public IActionResult Index()
        {
            var announcements = _context.Announcements?
                .OrderByDescending(a => a.DatePosted)
                .ToList() ?? new List<Announcement>(); // Ensure it is not null

            return View(announcements);
        }

        // Display Announcements to Users
        public IActionResult Announcement()
        {
            var announcements = _context.Announcements?
                .OrderByDescending(a => a.DatePosted)
                .ToList() ?? new List<Announcement>(); // Ensure it is not null

            return View("Announcement", announcements); // Render "Announcement.cshtml" with data
        }
    }
}
