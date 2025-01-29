using Microsoft.AspNetCore.Mvc;

namespace Orbitron.Controllers
{
    public class PackageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
