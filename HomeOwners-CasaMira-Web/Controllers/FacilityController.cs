using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Threading.Tasks;
using HomeOwners_CasaMira_Web.Data;
using HomeOwners_CasaMira_Web.Models;

namespace HomeOwners_CasaMira_Web.Controllers
{
    [Authorize(Roles = "Admin")]  // Restrict access to admins only
    public class FacilityController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FacilityController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Facility/Create
        public IActionResult Create()
        {
            return View();
        }

       // POST: Facility/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,IsAvailable")] Facility facility, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    // Save the image to the wwwroot/images folder
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    facility.ImageUrl = "/images/" + fileName; // Store the relative image URL
                }

                _context.Add(facility);
                await _context.SaveChangesAsync();

                // Set a success message
                TempData["SuccessMessage"] = "Facility created successfully!";

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

        // GET: Facility/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }

        // POST: Facility/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IsAvailable,ImageUrl")] Facility facility, IFormFile imageFile)
        {
            if (id != facility.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(facility.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, facility.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Save the new image
                        string fileName = Path.GetFileName(imageFile.FileName);
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        facility.ImageUrl = "css/images/" + fileName; // Store the relative image URL
                    }

                    _context.Update(facility);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilityExists(facility.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(facility);
        }

        // GET: Facility/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facility = await _context.Facilities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }

        // POST: Facility/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);

            if (facility == null)
            {
                return NotFound();
            }

            // Delete the image file if it exists
            if (!string.IsNullOrEmpty(facility.ImageUrl)) // Check if the ImageUrl is not null or empty
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, facility.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Facilities.Remove(facility);
            await _context.SaveChangesAsync();

            // Set a success message
            TempData["SuccessMessage"] = "Facility deleted successfully!";

            // Redirect to the Index page after successful deletion
            return RedirectToAction(nameof(Index));  // This will refresh the index page
        }

        private bool FacilityExists(int id)
        {
            return _context.Facilities.Any(e => e.Id == id);
        }
    }
}