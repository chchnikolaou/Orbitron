using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbitron.Data;
using Orbitron.Models;

namespace Orbitron.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var packages = await _context.Packages.ToListAsync();
            return View(packages);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                //TempData["ErrorMessage"] = "You are already logged in.";
                
                return RedirectToAction("Account", "Client");
            }

            return View();
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You are already logged in.";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult ValidateAFM()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You need to log in to access this page.";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
