using Microsoft.AspNetCore.Mvc;
using AppointmentApp.Models;
using AppointmentApp.Repositories;

namespace AppointmentApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAppointmentRepository _repo;

        public HomeController(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToAction("Login", "Account");

            return View(new Appointment());
        }

        [HttpPost]
        public async Task<IActionResult> Index(Appointment model)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid) return View(model);

            model.Date = model.Date.ToLocalTime();
            await _repo.Create(model);

            HttpContext.Session.SetString("appt_id", model.Id ?? "");
            HttpContext.Session.SetString("appt_date", model.Date.ToString("dd'//'MM'//'yyyy"));

            return RedirectToAction("Receipt");
        }

        public async Task<IActionResult> Receipt()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToAction("Login", "Account");

            var id = HttpContext.Session.GetString("appt_id");
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            var appt = (await _repo.GetAll()).FirstOrDefault(x => x.Id == id);
            if (appt == null) return RedirectToAction("Index");

            ViewBag.FormattedDate = HttpContext.Session.GetString("appt_date") ?? "";
            return View(appt);
        }
    }
}
