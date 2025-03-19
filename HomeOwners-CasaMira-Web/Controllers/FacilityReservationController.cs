using HomeOwners_CasaMira_Web.Data;
using HomeOwners_CasaMira_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HomeOwners_CasaMira_Web.Controllers
{
    public class FacilityReservationController : Controller
    {
        private readonly AppDbContext _context;

        public FacilityReservationController(AppbContext context)
        {
            _context = context;
        }

        // GET: FacilityReservations
        public async Task<IActionResult> Index()
        {
            var reservations = await _context.FacilityReservations.Include(r => r.Facility).ToListAsync();
            return View(reservations);
        }

        // GET: Create
        public IActionResult Create()
        {
            ViewBag.Facilities = _context.Facilities.ToList();
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FacilityReservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.FacilityReservations.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Facilities = _context.Facilities.ToList();
            return View(reservation);
        }

        // GET: Details
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _context.FacilityReservations
                .Include(r => r.Facility)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (reservation == null) return NotFound();

            return View(reservation);
        }

        // GET: Delete Confirmation
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.FacilityReservations.FindAsync(id);
            if (reservation == null) return NotFound();

            return View(reservation);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.FacilityReservations.FindAsync(id);
            if (reservation != null)
            {
                _context.FacilityReservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
