using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeOwners_CasaMira_Web.Data;
using HomeOwners_CasaMira_Web.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        public IActionResult Index()
        {
            // Define the list of request types for selection
            var requestTypes = new List<string>
            {
                "Plumbing",
                "Electrical",
                "HVAC",
                "Landscaping",
                "Security",
                "Common Area",
                "Other"
            };

            // Pass the request types to the view
            ViewBag.RequestTypes = requestTypes;
            
            return View();
        }

        // GET: ServiceRequest/MyRequests
        public async Task<IActionResult> MyRequests()
        {
            try
            {
                // Get the logged-in user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Fetch requests for the logged-in user
                var requests = await _context.ServiceRequests
                    .Where(sr => sr.UserId == userId)
                    .OrderByDescending(sr => sr.CreatedAt)
                    .ToListAsync();

                return View(requests);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MyRequests: {ex.Message}");
                TempData["ErrorMessage"] = $"Error retrieving service requests: {ex.Message}";
                return View(new List<ServiceRequest>());
            }
        }

        // GET: ServiceRequest/CreateForType/RequestType
        public IActionResult CreateForType(string requestType)
        {
            if (string.IsNullOrEmpty(requestType))
            {
                TempData["ErrorMessage"] = "Please select a request type.";
                return RedirectToAction(nameof(Index));
            }

            // Define the list of request types (for validation)
            var validRequestTypes = new List<string>
            {
                "Plumbing",
                "Electrical",
                "HVAC",
                "Landscaping",
                "Security",
                "Common Area",
                "Other"
            };

            if (!validRequestTypes.Contains(requestType))
            {
                TempData["ErrorMessage"] = "Invalid request type selected.";
                return RedirectToAction(nameof(Index));
            }

            // Set the selected request type in the ViewBag
            ViewBag.SelectedRequestType = requestType;
            
            // Get recent similar requests (for reference)
            var recentRequests = _context.ServiceRequests
                .Where(sr => sr.RequestType == requestType)
                .OrderByDescending(sr => sr.CreatedAt)
                .Take(3)
                .ToList();
                
            if (recentRequests.Any())
            {
                ViewBag.RecentRequests = recentRequests;
            }

            return View("Create");
        }

        // POST: ServiceRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string RequestType, string Location, string Description)
        {
            try
            {
                // Input validation
                if (string.IsNullOrEmpty(RequestType))
                {
                    TempData["ErrorMessage"] = "Please specify the request type.";
                    return RedirectToCreateWithType(RequestType);
                }

                if (string.IsNullOrEmpty(Location))
                {
                    TempData["ErrorMessage"] = "Please specify the location.";
                    return RedirectToCreateWithType(RequestType);
                }

                if (string.IsNullOrEmpty(Description))
                {
                    TempData["ErrorMessage"] = "Please provide a description of the issue.";
                    return RedirectToCreateWithType(RequestType);
                }

                // Create a new service request object
                var serviceRequest = new ServiceRequest
                {
                    RequestType = RequestType,
                    Location = Location,
                    Description = Description,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };

                try
                {
                    _context.ServiceRequests.Add(serviceRequest);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Service request submitted successfully!";
                    return RedirectToAction(nameof(MyRequests));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving service request: {ex.Message}");
                    TempData["ErrorMessage"] = "An error occurred while creating your service request.";
                    return RedirectToCreateWithType(RequestType);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create POST: {ex.Message}");
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private IActionResult RedirectToCreateWithType(string requestType)
        {
            return RedirectToAction(nameof(CreateForType), new { requestType = requestType });
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
                TempData["ErrorMessage"] = "Service request not found.";
                return RedirectToAction(nameof(MyRequests));
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
                return RedirectToAction(nameof(MyRequests));
            }

            // Only allow cancellation of pending requests
            if (serviceRequest.Status != "Pending")
            {
                TempData["ErrorMessage"] = "Only pending requests can be cancelled.";
                return RedirectToAction(nameof(MyRequests));
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

            return RedirectToAction(nameof(MyRequests));
        }
    }
} 