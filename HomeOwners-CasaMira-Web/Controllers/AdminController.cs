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

            var documents = _context.Documents
                .OrderByDescending(d => d.UploadedAt)
                .ToList();

            // Pass to view
            ViewBag.TotalFacilities = totalFacilities;
            ViewBag.ReservedFacilities = reservedFacilities;
            ViewBag.Documents = documents; // <- AND THIS LINE

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDocument(string Title, IFormFile DocumentFile)
        {
            if (DocumentFile == null || DocumentFile.Length == 0 || string.IsNullOrWhiteSpace(Title))
            {
                TempData["ErrorMessage"] = "Please provide a title and select a document to upload.";
                return RedirectToAction("Dashboard");
            }

            // Ensure the wwwroot/uploads directory exists
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Generate unique file name
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(DocumentFile.FileName)}";
            var fullPath = Path.Combine(uploadPath, uniqueFileName);

            // Save the file
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await DocumentFile.CopyToAsync(stream);
            }

            // Save document info to the database
            var document = new Document
            {
                Title = Title,
                FilePath = "/uploads/" + uniqueFileName,
                UploadedAt = DateTime.UtcNow
            };

            _context.Add(document);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Document uploaded successfully!";
            return RedirectToAction("Dashboard");
        }

        
            // Action to delete a document
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteDocument(int id)
            {
                var document = await _context.Documents.FindAsync(id);
                if (document != null)
                {
                    // Optionally, delete the physical file from storage if needed
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", document.FilePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    _context.Documents.Remove(document);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Document deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Document not found.";
                }

                return RedirectToAction("Dashboard"); // Redirect to the Admin Dashboard or appropriate view
            }


    }
}
