using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeOwners_CasaMira_Web.Data;
using HomeOwners_CasaMira_Web.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace HomeOwners_CasaMira_Web.Controllers
{
    [Authorize]
    public class FacilityReservationController : Controller
    {
        private readonly AppDbContext _context;

        public FacilityReservationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FacilityReservationController
        public IActionResult Index()
        {
            // Fetch all facilities and their details
            var facilities = _context.Facilities.ToList();

            // Pass the facilities to the view
            return View(facilities);
        }



       // GET: FacilityReservationController/MyReservations
        public IActionResult MyReservations()
        {
            // Get the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch reservations for the logged-in user
            var reservations = _context.FacilityReservations
                .Where(r => r.UserId == userId)
                .ToList();

            // Pass the reservations to the view
            return View(reservations);
        }

        // GET: FacilityReservationController/Create
        public IActionResult Create()
        {
            var availableFacilities = _context.Facilities
                .Where(f => f.IsAvailable)
                .ToList();

            if (!availableFacilities.Any())
            {
                Console.WriteLine("No available facilities found.");
            }
            else
            {
                foreach (var facility in availableFacilities)
                {
                    Console.WriteLine($"Facility: {facility.Name}, Available: {facility.IsAvailable}");
                }
            }

            ViewBag.AvailableFacilities = availableFacilities;

            return View();
        }

        // POST: FacilityReservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FacilityReservation reservation)
        {
            // Set the UserId to the currently logged-in user
            reservation.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Set the default status
            reservation.Status = "Pending";

            // Remove UserId and Status from ModelState validation
            ModelState.Remove(nameof(reservation.UserId));
            ModelState.Remove(nameof(reservation.Status));

            if (ModelState.IsValid)
            {
                // Check for overlapping reservations for the same facility
                var overlappingReservation = _context.FacilityReservations
                    .Where(r => r.FacilityId == reservation.FacilityId &&
                                r.ReservationDate == reservation.ReservationDate &&
                                ((reservation.StartTime >= r.StartTime && reservation.StartTime < r.EndTime) || // Overlaps start
                                 (reservation.EndTime > r.StartTime && reservation.EndTime <= r.EndTime) ||   // Overlaps end
                                 (reservation.StartTime <= r.StartTime && reservation.EndTime >= r.EndTime))) // Fully overlaps
                    .FirstOrDefault();

                if (overlappingReservation != null)
                {
                    // Add an error message if there is an overlap
                    ModelState.AddModelError(string.Empty, "The selected time slot is already reserved for this facility.");
                }
                else
                {
                    // Add the reservation to the database
                    _context.FacilityReservations.Add(reservation);

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Reservation created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }

            // Reload available facilities if the model is invalid
            ViewBag.AvailableFacilities = _context.Facilities
                .Where(f => f.IsAvailable)
                .ToList();

            return View(reservation);
        }

        // GET: FacilityReservationController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: FacilityReservationController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: FacilityReservationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FacilityReservation reservation)
        {
            // Placeholder for reservation editing logic (removed).
            return RedirectToAction(nameof(Index));
        }

        // GET: FacilityReservationController/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: FacilityReservationController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Placeholder for reservation deletion logic (removed).
            return RedirectToAction(nameof(Index));
        }
    }
}
