using System.Diagnostics;
using HomeOwners_CasaMira_Web.Data;

using HomeOwners_CasaMira_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HomeOwners_CasaMira_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;  // Declare the context

        // Inject the context in the constructor
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;  // Initialize the context
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Ameneties()
        {
            return View();
        }

        [Authorize]
        public IActionResult Communications()
        {
            return View();
        }

        [Authorize]
        public IActionResult OurCommunity()
        {
            return View();
        }

        public IActionResult Payments()
        {
            return View();
        }

        // Action to fetch the uploaded documents
        public IActionResult ViewDocuments()
        {
            var documents = _context.Documents.ToList(); // Fetch documents from the context
            ViewBag.Documents = documents;  // Pass the documents to the view
            return View();
        }

        public static List<EmergencyContact> StaticEmergencyContacts = new List<EmergencyContact>
        {
            new EmergencyContact { Id = 1, Name = "Police Station", Type = "Police", PhoneNumber = "911", Description = "Local police emergency hotline" },
            new EmergencyContact { Id = 2, Name = "Fire Department", Type = "Fire", PhoneNumber = "922", Description = "Local fire department emergency hotline" },
            new EmergencyContact { Id = 3, Name = "Medical Emergency", Type = "Medical", PhoneNumber = "933", Description = "Nearest hospital emergency room" },
            new EmergencyContact { Id = 4, Name = "Subdivision Security", Type = "Security", PhoneNumber = "900-123-4567", Description = "On-site subdivision security office" }
        };

        public IActionResult EmergencyContacts()
        {
            ViewBag.EmergencyContacts = StaticEmergencyContacts;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
