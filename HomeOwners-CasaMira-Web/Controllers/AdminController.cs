using HomeOwners_CasaMira_Web.Data;
using HomeOwners_CasaMira_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;

namespace HomeOwners_CasaMira_Web.Controllers
{
    [Authorize(Roles = "Admin")] // Only admins can access
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public AdminController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Dashboard()
        {
            // --- User Analytics ---
            var allUsers = _userManager.Users.ToList();
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var start6MonthsAgo = startOfMonth.AddMonths(-5);

            var totalUsers = allUsers.Count;
            var newUsersThisMonth = allUsers.Count(u => u.DateOfBirth >= startOfMonth); // Replace with CreatedAt if available

            // --- Forum Analytics ---
            var allPosts = _context.ForumPosts.ToList();
            var allComments = _context.ForumComments.ToList();
            var totalPosts = allPosts.Count;
            var postsThisMonth = allPosts.Count(p => p.CreatedAt >= startOfMonth);
            var totalComments = allComments.Count;
            var commentsThisMonth = allComments.Count(c => c.CreatedAt >= startOfMonth);

            // --- Document Analytics ---
            var allDocuments = _context.Documents.ToList();
            var totalDocuments = allDocuments.Count;
            var documentsThisMonth = allDocuments.Count(d => d.UploadedAt >= startOfMonth);

            // --- Reservation Analytics ---
            var allReservations = _context.FacilityReservations.ToList();
            var totalReservations = allReservations.Count;
            var reservationsThisMonth = allReservations.Count(r => r.ReservationDate >= startOfMonth);

            // --- User Growth (last 6 months) ---
            var userGrowthLabels = new List<string>();
            var userGrowthData = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                var month = start6MonthsAgo.AddMonths(i);
                userGrowthLabels.Add(month.ToString("MMM yyyy"));
                userGrowthData.Add(allUsers.Count(u => u.DateOfBirth >= new DateTime(month.Year, month.Month, 1) && u.DateOfBirth < new DateTime(month.Year, month.Month, 1).AddMonths(1)));
            }

            // --- Post Activity (last 6 months) ---
            var postActivityLabels = new List<string>();
            var postActivityData = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                var month = start6MonthsAgo.AddMonths(i);
                postActivityLabels.Add(month.ToString("MMM yyyy"));
                postActivityData.Add(allPosts.Count(p => p.CreatedAt >= new DateTime(month.Year, month.Month, 1) && p.CreatedAt < new DateTime(month.Year, month.Month, 1).AddMonths(1)));
            }

            // --- Reservation Trends (last 6 months) ---
            var reservationTrendsLabels = new List<string>();
            var reservationTrendsData = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                var month = start6MonthsAgo.AddMonths(i);
                reservationTrendsLabels.Add(month.ToString("MMM yyyy"));
                reservationTrendsData.Add(allReservations.Count(r => r.ReservationDate >= new DateTime(month.Year, month.Month, 1) && r.ReservationDate < new DateTime(month.Year, month.Month, 1).AddMonths(1)));
            }

            // Pass to view
            ViewBag.TotalUsers = totalUsers;
            ViewBag.NewUsersThisMonth = newUsersThisMonth;
            ViewBag.TotalPosts = totalPosts;
            ViewBag.PostsThisMonth = postsThisMonth;
            ViewBag.TotalComments = totalComments;
            ViewBag.CommentsThisMonth = commentsThisMonth;
            ViewBag.TotalDocuments = totalDocuments;
            ViewBag.DocumentsThisMonth = documentsThisMonth;
            ViewBag.TotalReservations = totalReservations;
            ViewBag.ReservationsThisMonth = reservationsThisMonth;
            ViewBag.UserGrowthData = new { labels = userGrowthLabels, data = userGrowthData };
            ViewBag.PostActivityData = new { labels = postActivityLabels, data = postActivityData };
            ViewBag.ReservationTrendsData = new { labels = reservationTrendsLabels, data = reservationTrendsData };

            // Existing facility and document logic
            var totalFacilities = _context.Facilities.Count();
            var reservedFacilities = _context.FacilityReservations
                .GroupBy(fr => fr.Facility.Name)
                .Select(g => new
                {
                    FacilityName = g.Key,
                    ReservationCount = g.Count()
                })
                .ToList();
            var documents = _context.Documents.OrderByDescending(d => d.UploadedAt).ToList();
            ViewBag.TotalFacilities = totalFacilities;
            ViewBag.ReservedFacilities = reservedFacilities;
            ViewBag.Documents = documents;

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
