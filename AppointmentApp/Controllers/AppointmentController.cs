using Microsoft.AspNetCore.Mvc;
using AppointmentApp.Models;
using AppointmentApp.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApp.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository _repo;

        public AppointmentController(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        // GET: /Appointment/Index
        [HttpGet]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToAction("Login", "Account");

            return View(new Appointment());
        }

        // POST: /Appointment/Index
        [HttpPost]
        public async Task<IActionResult> Index(Appointment model)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            await _repo.Create(model);

            // Save ID in session for receipt
            HttpContext.Session.SetString("appt_id", model.Id ?? "");

            return RedirectToAction("Receipt", new { id = model.Id });
        }

        // GET: /Appointment/Receipt/{id}
        [HttpGet]
        public async Task<IActionResult> Receipt(string id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var appt = await _repo.GetById(id);
            if (appt == null)
                return RedirectToAction("Index");

            return View(appt);
        }
    }
}

