using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeOwners_CasaMira_Web.Controllers
{
    public class FacilityReservationController : Controller
    {
        // GET: FacilityReservationController
        public ActionResult Index()
        {
            return View();
        }

        // GET: FacilityReservationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FacilityReservationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FacilityReservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FacilityReservationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FacilityReservationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FacilityReservationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FacilityReservationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
