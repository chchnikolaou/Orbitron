using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Orbitron.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Orbitron.Models;
using Orbitron.Models.ViewModels;
using System.Reflection.Metadata.Ecma335;

namespace Orbitron.Controllers
{
    public class ClientController : Controller
    {
        private readonly DataContext _context;

        public ClientController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Account()
        {
            if (User.Identity == null)
            {
                TempData["ErrorMessage"] = "You need to log in to access this page.";
                return RedirectToAction("Login", "Home");
            }

            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You need to log in to access this page.";
                return RedirectToAction("Login", "Home");
            }

            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (role == null)
            {
                TempData["ErrorMessage"] = "There was an error. Please contact an administrator.";
                return RedirectToAction("Index", "Home");
            }
            if (!role.ToLower().Equals("client"))
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index", "Home");
            }

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                TempData["ErrorMessage"] = "Invalid user data. Please contact an administrator.";
                return RedirectToAction("Index", "Home");
            }

            Client client = await getClient(userId);
            if (client == null || client.AFM == string.Empty)
            {
                TempData["InfoMessage"] = "Please specify your ΑΦΜ.";
                return RedirectToAction("ValidateAFM", "Home");
            }

            Phone phone = client?.PhoneNumber != null ? await getPhone(client.PhoneNumber) : null;
            Package package = phone?.Package != null ? await getPackage(phone.Package) : null;

            ClientPhoneViewModel viewModel = new ClientPhoneViewModel
            {
                Client = client,
                Phone = phone,
                Package = package
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> History()
        {

            if (User.Identity == null)
            {
                TempData["ErrorMessage"] = "You need to log in to access this page.";
                return RedirectToAction("Login", "Home");
            }

            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You need to log in to access this page.";
                return RedirectToAction("Login", "Home");
            }

            string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (role == null)
            {
                TempData["ErrorMessage"] = "There was an error. Please contact an administrator.";
                return RedirectToAction("Index", "Home");
            }
            if (!role.ToLower().Equals("client"))
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index", "Home");
            }

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                TempData["ErrorMessage"] = "Invalid user data. Please contact an administrator.";
                return RedirectToAction("Index", "Home");
            }

            Client client = await getClient(userId);
            if (client == null || client.AFM == string.Empty)
            {
                TempData["InfoMessage"] = "Please specify your ΑΦΜ.";
                return RedirectToAction("ValidateAFM", "Home");
            }

            List<Call> calls = await getCalls(client);

            CallClientViewModel model = new CallClientViewModel
            {
                Client = client,
                Calls = calls
            };


            return View(model);
        }

        public async Task<IActionResult> Bill()
        {

            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You need to log in to access this page.";
                return RedirectToAction("Login", "Home");
            }

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                TempData["ErrorMessage"] = "Invalid user data. Please contact an administrator.";
                return RedirectToAction("Index", "Home");
            }


            Client client = await getClient(userId);
            if (client == null)
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index", "Home");
            }

            Phone phone = client?.PhoneNumber != null ? await getPhone(client.PhoneNumber) : null;
            Package package = phone?.Package != null ? await getPackage(phone.Package) : null;

            ClientPhoneViewModel viewModel = new ClientPhoneViewModel
            {
                Client = client,
                Phone = phone,
                Package = package
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SetAFM([Bind("AFM")] string afm)
        {
            if(!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You need to log in to access this page.";
                return RedirectToAction("Login", "Home");
            }

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                TempData["ErrorMessage"] = "Invalid user data. Please contact an administrator.";
                return RedirectToAction("Index", "Home");
            }


            Client client = await getClient(userId);
            if (client == null)
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index", "Home");
            }

            client.AFM = afm;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "You have successfully registered your ΑΦΜ.";
            return RedirectToAction("Account", this);
        }


        public async Task<IActionResult> Pay() 
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You need to log in to access this page.";
                return RedirectToAction("Login", "Home");
            }

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                TempData["ErrorMessage"] = "Invalid user data. Please contact an administrator.";
                return RedirectToAction("Index", "Home");
            }


            Client client = await getClient(userId);
            if (client == null)
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index", "Home");
            }

            if(string.IsNullOrEmpty(client.PhoneNumber))
            {
                TempData["ErrorMessage"] = "You do not have an active phone number.";
                return RedirectToAction("Bill", "Client");
            }

            Phone phone = await getPhone(client.PhoneNumber);
            if(phone==null)
            {
                TempData["ErrorMessage"] = "You do not have an active phone number.";
                return RedirectToAction("Bill", "Client");
            }

            if(string.IsNullOrEmpty(phone.Package))
            {
                TempData["ErrorMessage"] = "You do not have an active package.";
                return RedirectToAction("Bill", "Client");
            }

            Package package = await getPackage(phone.Package);
            if(package==null)
            {
                TempData["ErrorMessage"] = "You do not have an active package.";
                return RedirectToAction("Bill", "Client");
            }

            double cost = package.Cost;
            phone.renewDate = phone.renewDate.AddMonths(1);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "You have successfully paid " + cost + "€";

            return RedirectToAction("Bill", "Client");
        }


        private async Task<bool> ClientExists(int userId)
        {
            return await _context.Clients.AnyAsync(u => u.Id == userId);
        }

        private async Task<Client> getClient(int userId) => await _context.Clients.FirstOrDefaultAsync(u => u.Id == userId);
        private async Task<Phone> getPhone(string phone) => await _context.Phones.FirstOrDefaultAsync(p => p.Number == phone);
        private async Task<Package> getPackage(string package) => await _context.Packages.FirstOrDefaultAsync(p => p.Name == package);
        private async Task<List<Call>> getCalls(Client client) => await _context.Calls.Where(c=>client.PhoneNumber != null && c.PhoneSender == client.PhoneNumber).ToListAsync();
    }
}
