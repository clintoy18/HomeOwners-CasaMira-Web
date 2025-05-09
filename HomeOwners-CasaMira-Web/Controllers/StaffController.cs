using HomeOwners_CasaMira_Web.Data;
using HomeOwners_CasaMira_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HomeOwners_CasaMira_Web.Controllers
{
    [Authorize(Roles = "Staff")] // Only staff can access
    public class StaffController : Controller
    {
        private readonly AppDbContext _context;

        public StaffController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public new IActionResult Request()
        {
            return View();
        }
        
        // GET: Staff/ServiceRequests
        public async Task<IActionResult> ServiceRequests()
        {
            var requests = await _context.ServiceRequests
                .Include(s => s.User)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
                
            return View(requests);
        }
        
        // GET: Staff/ServiceRequestDetails/5
        public async Task<IActionResult> ServiceRequestDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.ServiceRequests
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }
        
        // GET: Staff/UpdateServiceRequest/5
        public async Task<IActionResult> UpdateServiceRequest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.ServiceRequests
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (request == null)
            {
                return NotFound();
            }

            ViewBag.StatusOptions = new List<string> 
            { 
                "Pending", 
                "In Progress", 
                "Completed", 
                "Cancelled" 
            };

            return View(request);
        }
        
        // POST: Staff/UpdateServiceRequest/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateServiceRequest(int id, [Bind("Id,Status,StaffNotes")] ServiceRequest requestUpdate)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            
            if (request == null)
            {
                return NotFound();
            }
            
            try
            {
                request.Status = requestUpdate.Status;
                request.StaffNotes = requestUpdate.StaffNotes;
                request.UpdatedAt = DateTime.Now;
                
                _context.Update(request);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Service request updated successfully!";
                return RedirectToAction(nameof(ServiceRequests));
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the request.";
                return RedirectToAction(nameof(ServiceRequests));
            }
        }
    }
}
