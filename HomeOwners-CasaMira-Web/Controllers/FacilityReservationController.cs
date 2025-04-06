using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeOwners_CasaMira_Web.Data;
using HomeOwners_CasaMira_Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace HomeOwners_CasaMira_Web.Controllers
{
    [Authorize]
    public class FacilityReservationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public FacilityReservationController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: FacilityReservationController
        public async Task<IActionResult> Index()
        {
            var reservations = await _context.FacilityReservations
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();

            return View(reservations);
        }

        // GET: FacilityReservationController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FacilityReservationController/Create
      [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(FacilityReservation reservation)
{
    if (!ModelState.IsValid)
    {
        TempData["Error"] = "Please fill out all required fields.";
        return View(reservation); // If validation fails, stay on the Create page
    }

    // Set the UserId from the logged-in user
    reservation.UserId = _userManager.GetUserId(User);

    // Check for conflict (using asynchronous call)
    bool hasConflict = await _context.FacilityReservations.AnyAsync(r =>
        r.FacilityId == reservation.FacilityId &&
        r.ReservationDate == reservation.ReservationDate &&
        r.Status != "Cancelled" &&
        (
            (reservation.StartTime >= r.StartTime && reservation.StartTime < r.EndTime) ||
            (reservation.EndTime > r.StartTime && reservation.EndTime <= r.EndTime) ||
            (reservation.StartTime <= r.StartTime && reservation.EndTime >= r.EndTime)
        ));

    if (hasConflict)
    {
        if (Request.IsAjaxRequest()) // If it's an AJAX request
        {
            return Json(new { success = false, message = "This timeslot is already reserved." });
        }
        TempData["Error"] = "This timeslot is already reserved.";
        return View(reservation); // If it's not an AJAX request, stay on the Create page
    }

    _context.FacilityReservations.Add(reservation);
    await _context.SaveChangesAsync();

    if (Request.IsAjaxRequest()) // If it's an AJAX request
    {
        return Json(new { success = true, message = "Reservation submitted successfully." });
    }

    // If it's a normal request (non-AJAX), redirect to Index
    TempData["Success"] = "Reservation submitted successfully.";
    return RedirectToAction(nameof(Index)); // Redirect to Index
}

        // GET: FacilityReservationController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _context.FacilityReservations.FirstOrDefaultAsync(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: FacilityReservationController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _context.FacilityReservations.FirstOrDefaultAsync(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: FacilityReservationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FacilityReservation updated)
        {
            var existing = await _context.FacilityReservations.FirstOrDefaultAsync(r => r.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(updated);
            }

            // Update only the necessary fields
            existing.ReservationDate = updated.ReservationDate;
            existing.StartTime = updated.StartTime;
            existing.EndTime = updated.EndTime;
            existing.Status = updated.Status;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Reservation updated.";
            return RedirectToAction(nameof(Index));
        }

        // GET: FacilityReservationController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.FacilityReservations.FirstOrDefaultAsync(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: FacilityReservationController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.FacilityReservations.FirstOrDefaultAsync(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.FacilityReservations.Remove(reservation);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Reservation deleted.";
            return RedirectToAction(nameof(Index));
        }
    }

    // Extension Method to check for AJAX request
    public static class ControllerExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
