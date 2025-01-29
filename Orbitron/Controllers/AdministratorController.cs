using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Orbitron.Data;
using Orbitron.Models;
using System.Security.Claims;

namespace Orbitron.Controllers
{
    public class AdministratorController : Controller
    {

        private readonly DataContext _context;
        public AdministratorController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You need to log in to access this page.";
                return RedirectToAction("Login", "Home");
            }

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int selfId))
            {
                TempData["ErrorMessage"] = "Invalid user data. Please contact an administrator.";
                return RedirectToAction("Index", "Home");
            }

            Administrator admin = await getAdmin(selfId);
            if (admin == null)
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        public async Task<IActionResult> AddSeller()
        {

            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You need to log in to access this page.";
                return RedirectToAction("Login", "Home");
            }

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int selfId))
            {
                TempData["ErrorMessage"] = "Invalid user data. Please contact an administrator.";
                return RedirectToAction("Index", "Home");
            }

            Administrator admin = await getAdmin(selfId);
            if (admin == null)
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        public async Task<IActionResult> AddPackage()
        {

            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You need to log in to access this page.";
                return RedirectToAction("Login", "Home");
            }

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int selfId))
            {
                TempData["ErrorMessage"] = "Invalid user data. Please contact an administrator.";
                return RedirectToAction("Index", "Home");
            }

            Administrator admin = await getAdmin(selfId);
            if (admin == null)
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        public async Task<IActionResult> AlternatePackage()
        {

            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You need to log in to access this page.";
                return RedirectToAction("Login", "Home");
            }

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int selfId))
            {
                TempData["ErrorMessage"] = "Invalid user data. Please contact an administrator.";
                return RedirectToAction("Index", "Home");
            }

            Administrator admin = await getAdmin(selfId);
            if (admin == null)
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index", "Home");
            }


            var packages = await _context.Packages.ToListAsync();
            return View(packages);
        }


        [HttpPost]
        public async Task<IActionResult> AddSellerAction(int userid)
        {

            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You need to log in to access this page.";
                return RedirectToAction("Login", "Home");
            }

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int selfId))
            {
                TempData["ErrorMessage"] = "Invalid user data. Please contact an administrator.";
                return RedirectToAction("Index", "Home");
            }

            Administrator admin = await getAdmin(selfId);
            if (admin == null)
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index", "Home");
            }

            User user = await getUser(userid);
            if(user==null)
            {
                TempData["ErrorMessage"] = "This user does not exist.";
                return RedirectToAction("AddSeller", "Administrator");
            }
            if (user.Property == Property.Seller)
            {
                TempData["ErrorMessage"] = "This user is already a seller.";
                return RedirectToAction("AddSeller", "Administrator");
            }
            _context.Users.Remove(user);

            _context.Sellers.Add(new Seller { 
                Id = userid,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password, 
                Username = user.Username, 
                Property = Property.Seller
            });

            _context.SaveChanges();

            TempData["SuccessMessage"] = "All changes were successfully saved.";
            return RedirectToAction("AddSeller", "Administrator");


        }



        [HttpPost]
        public async Task<IActionResult> AddPackageAction(string name, string description, int totalMinutes, int totalMinutesAbroad, 
            int sms, int mb, string cost)
        {

            if(name.IsNullOrEmpty() || description.IsNullOrEmpty())
            {

                TempData["ErrorMessage"] = "Name/Description was not specified.";
                return RedirectToAction("AddPackage");
            }

            Package package = await getPackage(name);
            if (package != null)
            {
                TempData["ErrorMessage"] = "Package with that name already exists.";
                return RedirectToAction("AddPackage");
            }

            double realCost = -1;
            try
            {
                realCost = Double.Parse(cost);
            } catch(Exception e)
            {
                TempData["ErrorMessage"] = "Please specify a valid cost.";
                return RedirectToAction("AddPackage");
            }

            if (realCost <= 0)
            {
                TempData["ErrorMessage"] = "Please specify a valid cost.";
                return RedirectToAction("AddPackage");
            }

            package = new Package
            {
                Name = name,
                Description = description,
                TotalMinutes = totalMinutes,
                TotalMinutesAbroad = totalMinutesAbroad,
                SMS = sms,
                MB = mb,
                Cost = realCost
            };

            _context.Packages.Add(package);
            _context.SaveChanges();


            TempData["SuccessMessage"] = "Package " + name + " has been created successfully.";
            return RedirectToAction("AddPackage");
        }

        [HttpPost]
        public async Task<IActionResult> AlternatePackageAction(string name, string description, int totalMinutes, int totalMinutesAbroad,
     int sms, int mb, string cost)
        {

            if (name.IsNullOrEmpty() || description.IsNullOrEmpty())
            {

                TempData["ErrorMessage"] = "Name/Description was not specified.";
                return RedirectToAction("AlternatePackage");
            }

            Package package = await getPackage(name);
            if (package == null)
            {
                TempData["ErrorMessage"] = "Package with that name does not exist.";
                return RedirectToAction("AlternatePackage");
            }

            double realCost = -1;
            try
            {
                realCost = Double.Parse(cost);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Please specify a valid cost.";
                return RedirectToAction("AlternatePackage");
            }

            if (realCost <= 0)
            {
                TempData["ErrorMessage"] = "Please specify a valid cost.";
                return RedirectToAction("AlternatePackage");
            }

            package.Description = description;
            package.TotalMinutes = totalMinutes;
            package.TotalMinutesAbroad = totalMinutesAbroad;
            package.SMS = sms;
            package.MB = mb;
            package.Cost = realCost;
            

            _context.SaveChanges();


            TempData["SuccessMessage"] = "All changes were successfully saved.";
            return RedirectToAction("AlternatePackage");
        }



        private async Task<User> getUser(int userId) => await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        private async Task<Administrator> getAdmin(int userId) => await _context.Administrators.FirstOrDefaultAsync(s => s.Id == userId);
        private async Task<Client> getClient(string afm) => await _context.Clients.FirstOrDefaultAsync(u => u.AFM == afm);
        private async Task<Client> getClientByUsername(string username) => await _context.Clients.FirstOrDefaultAsync(u => u.Username == username);
        private async Task<Phone> getPhone(string phone) => await _context.Phones.FirstOrDefaultAsync(p => p.Number == phone);
        private async Task<List<Call>> getCalls(string phone, DateTime fromDate, DateTime nextDate) => await _context.Calls.Where(c => c.PhoneSender == phone
        && (c.Started >= fromDate && c.Started <= nextDate) && (c.Ended >= fromDate && c.Ended <= nextDate)
        ).ToListAsync();
        private async Task<Package> getPackage(string package) => await _context.Packages.FirstOrDefaultAsync(p => p.Name == package);

    }
}
