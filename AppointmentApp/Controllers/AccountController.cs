using AppointmentApp.Models;
using AppointmentApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using BCrypt.Net;

namespace AppointmentApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepo;

        public AccountController(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new User());
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _userRepo.Exists(model.Email))
            {
                ModelState.AddModelError("", "Email already registered.");
                return View(model);
            }

            // Hash password
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var success = await _userRepo.Register(model);
            if (!success)
            {
                ModelState.AddModelError("", "Registration failed. Try again.");
                return View(model);
            }

            // Store user ID and email in session
            HttpContext.Session.SetString("UserId", model.Id ?? "");
            HttpContext.Session.SetString("UserEmail", model.Email);
            HttpContext.Session.SetString("UserName", model.Name ?? "");

            return RedirectToAction("Index", "Appointment");
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                return View();
            }

            var users = await _userRepo.GetAll();
            var user = users.FirstOrDefault(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }

            // Store user info in session
            HttpContext.Session.SetString("UserId", user.Id ?? "");
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.Name ?? "");

            // Redirect to Appointment page after login
            return RedirectToAction("Index", "Appointment");
        }

        // GET: /Account/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear all session data
            return RedirectToAction("Login");
        }
    }
}

