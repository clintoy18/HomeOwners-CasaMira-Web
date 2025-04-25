using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeOwners_CasaMira_Web.Data;
using HomeOwners_CasaMira_Web.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System;
using Microsoft.EntityFrameworkCore;

namespace HomeOwners_CasaMira_Web.Controllers
{
    [Authorize]
    public class ServiceRequestController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceRequestController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ServiceRequest
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var requests = await _context.ServiceRequests
                .Where(sr => sr.UserId == userId)
                .OrderByDescending(sr => sr.CreatedAt)
                .ToListAsync();

            return View(requests);
        }

        // GET: ServiceRequest/Create
        public IActionResult Create()
        {
            // Define the list of request types
            ViewBag.RequestTypes = new List<string> 
            { 
                "Plumbing", 
                "Electrical", 
                "HVAC", 
                "Landscaping", 
                "Security", 
                "Common Area", 
                "Other" 
            };
            
            return View();
        }

        // POST: ServiceRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestType,Description,Location")] ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                // Set the current user as the request owner
                serviceRequest.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                serviceRequest.Status = "Pending";
                serviceRequest.CreatedAt = DateTime.Now;

                try
                {
                    _context.Add(serviceRequest);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Service request submitted successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }

            // If we got to here, something failed, redisplay form
            ViewBag.RequestTypes = new List<string> 
            { 
                "Plumbing", 
                "Electrical", 
                "HVAC", 
                "Landscaping", 
                "Security", 
                "Common Area", 
                "Other" 
            };
            
            return View(serviceRequest);
        }

        // GET: ServiceRequest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var serviceRequest = await _context.ServiceRequests
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
                
            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // POST: ServiceRequest/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var serviceRequest = await _context.ServiceRequests
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
                
            if (serviceRequest == null)
            {
                TempData["ErrorMessage"] = "Service request not found.";
                return RedirectToAction(nameof(Index));
            }

            // Only allow cancellation of pending requests
            if (serviceRequest.Status != "Pending")
            {
                TempData["ErrorMessage"] = "Only pending requests can be cancelled.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                serviceRequest.Status = "Cancelled";
                serviceRequest.UpdatedAt = DateTime.Now;
                _context.Update(serviceRequest);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Service request cancelled successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while cancelling the request: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 