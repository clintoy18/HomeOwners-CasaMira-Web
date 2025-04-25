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


        // GET: FacilityReservationController/Calendar

        public IActionResult Calendar()
        {
            // Fetch all reservations
            var reservations = _context.FacilityReservations
                .Select(r => new
                {
                    Title = GetFacilityName(r.FacilityId),
                    Start = r.ReservationDate.Add(r.StartTime),
                    End = r.ReservationDate.Add(r.EndTime),
                    Status = r.Status
                })
                .ToList();

            // Pass reservations as JSON to the view
            ViewBag.Reservations = System.Text.Json.JsonSerializer.Serialize(reservations);

            return View();
        }

        private string GetFacilityName(int facilityId)
        {
            var facility = _context.Facilities.FirstOrDefault(f => f.Id == facilityId);
            return facility != null ? facility.Name : "Unknown Facility";
        }

        // GET: FacilityReservationController/Create
        public IActionResult Create()
        {
            var availableFacilities = _context.Facilities
                .Where(f => f.IsAvailable)
                .ToList();

            ViewBag.AvailableFacilities = availableFacilities;

            return View();
        }

        // GET: FacilityReservationController/Create/FacilityId
        public IActionResult CreateForFacility(int? facilityId)
        {
            Console.WriteLine($"CreateForFacility called with facilityId: {facilityId}");
            
            if (facilityId == null || facilityId <= 0)
            {
                Console.WriteLine("Invalid facilityId: null or zero/negative");
                return RedirectToAction("Index");
            }

            var facility = _context.Facilities.FirstOrDefault(f => f.Id == facilityId);
            
            if (facility == null)
            {
                Console.WriteLine($"No facility found with ID: {facilityId}");
                
                // Debug: List all facilities in database
                var allFacilities = _context.Facilities.ToList();
                Console.WriteLine($"Total facilities in database: {allFacilities.Count}");
                foreach (var f in allFacilities)
                {
                    Console.WriteLine($"Available facility: ID={f.Id}, Name={f.Name}");
                }
                
                TempData["ErrorMessage"] = $"Facility with ID {facilityId} not found.";
                return RedirectToAction("Index");
            }

            Console.WriteLine($"Found facility: ID={facility.Id}, Name={facility.Name}");
            
            // Pass the selected facility to the view
            ViewBag.SelectedFacility = facility;

            // Get existing reservations for this facility
            var existingReservations = _context.FacilityReservations
                .Where(r => r.FacilityId == facilityId && r.ReservationDate.Date >= DateTime.Today)
                .OrderBy(r => r.ReservationDate)
                .ThenBy(r => r.StartTime)
                .Take(5)
                .ToList();

            if (existingReservations.Any())
            {
                ViewBag.ExistingReservations = existingReservations;
            }

            return View("Create");
        }

        // POST: FacilityReservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int FacilityId, DateTime ReservationDate, string StartTime, string EndTime)
        {
            Console.WriteLine($"Create POST called with FacilityId: {FacilityId}");
            Console.WriteLine($"ReservationDate: {ReservationDate}");
            Console.WriteLine($"StartTime: {StartTime}");
            Console.WriteLine($"EndTime: {EndTime}");
            
            // Input validation
            if (string.IsNullOrEmpty(StartTime) || string.IsNullOrEmpty(EndTime))
            {
                TempData["ErrorMessage"] = "Please specify both start and end times for your reservation.";
                return RedirectToCreateWithFacility(FacilityId);
            }

            TimeSpan startTimeSpan, endTimeSpan;
            try
            {
                startTimeSpan = TimeSpan.Parse(StartTime);
                endTimeSpan = TimeSpan.Parse(EndTime);
            }
            catch
            {
                TempData["ErrorMessage"] = "Invalid time format. Please use HH:MM format.";
                return RedirectToCreateWithFacility(FacilityId);
            }

            // Check if end time is after start time
            if (endTimeSpan <= startTimeSpan)
            {
                TempData["ErrorMessage"] = "End time must be later than start time.";
                return RedirectToCreateWithFacility(FacilityId);
            }
            
            // Check if reservation date is in the past
            if (ReservationDate.Date < DateTime.Today)
            {
                TempData["ErrorMessage"] = "Cannot make reservations for past dates.";
                return RedirectToCreateWithFacility(FacilityId);
            }

            // Create a new reservation object
            var reservation = new FacilityReservation
            {
                FacilityId = FacilityId,
                ReservationDate = ReservationDate,
                StartTime = startTimeSpan,
                EndTime = endTimeSpan,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Status = "Approved"
            };

            if (FacilityId <= 0)
            {
                TempData["ErrorMessage"] = "Please select a facility for your reservation.";
                return RedirectToAction("Index");
            }

            var facility = await _context.Facilities.FindAsync(FacilityId);
            if (facility == null)
            {
                TempData["ErrorMessage"] = $"Facility with ID {FacilityId} not found.";
                return RedirectToAction("Index");
            }

            // Check for overlapping reservations
            var overlappingReservation = await _context.FacilityReservations
                .Where(r => r.FacilityId == reservation.FacilityId &&
                            r.ReservationDate.Date == reservation.ReservationDate.Date &&
                            ((reservation.StartTime >= r.StartTime && reservation.StartTime < r.EndTime) ||
                            (reservation.EndTime > r.StartTime && reservation.EndTime <= r.EndTime) ||
                            (reservation.StartTime <= r.StartTime && reservation.EndTime >= r.EndTime)))
                .FirstOrDefaultAsync();

            if (overlappingReservation != null)
            {
                var conflictStartTime = overlappingReservation.StartTime.ToString(@"hh\:mm");
                var conflictEndTime = overlappingReservation.EndTime.ToString(@"hh\:mm");
                TempData["ErrorMessage"] = $"This time slot is already reserved. There is a booking from {conflictStartTime} to {conflictEndTime} on the selected date.";
                return RedirectToCreateWithFacility(FacilityId);
            }

            try
            {
                _context.FacilityReservations.Add(reservation);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Reservation created successfully!";
                return RedirectToAction(nameof(MyReservations));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving reservation: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while creating your reservation.";
                return RedirectToCreateWithFacility(FacilityId);
            }
        }

        private IActionResult RedirectToCreateWithFacility(int facilityId)
        {
            var facility = _context.Facilities.FirstOrDefault(f => f.Id == facilityId);
            if (facility != null)
            {
                ViewBag.SelectedFacility = facility;
                
                // Get existing reservations for this facility
                var existingReservations = _context.FacilityReservations
                    .Where(r => r.FacilityId == facilityId && r.ReservationDate.Date >= DateTime.Today)
                    .OrderBy(r => r.ReservationDate)
                    .ThenBy(r => r.StartTime)
                    .Take(5)
                    .ToList();

                if (existingReservations.Any())
                {
                    ViewBag.ExistingReservations = existingReservations;
                }
                
                return View("Create");
            }
            else
            {
                return RedirectToAction("Index");
            }
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