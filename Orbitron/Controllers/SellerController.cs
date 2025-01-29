using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Orbitron.Data;
using Orbitron.Models;
using Orbitron.Models.ViewModels;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace Orbitron.Controllers
{
    public class SellerController : Controller
    {

        private readonly DataContext _context;
        public SellerController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> AddClient()

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

            Seller seller = await getSeller(userId);
            if (seller == null)
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index", "Home");
            }

            var packages = await _context.Packages.ToListAsync();
            return View(packages);
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> IssueBill(string afm, string fromdate, string nextdate)
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

            Seller seller = await getSeller(userId);
            if (seller == null)
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index", "Home");
            }


            if (afm.IsNullOrEmpty() || fromdate.IsNullOrEmpty() || nextdate.IsNullOrEmpty())
            {
                CallClientViewModel model = new CallClientViewModel();
                TempData["ErrorMessage"] = "Please specify the required information.";
                return View(model);
            }

            Client client = await getClient(afm);
            if (client == null)
            {
                TempData["ErrorMessage"] = "The client was not found.";
                return RedirectToAction("IssueBill", "Seller");
            }

            List<Call> calls = await getCalls(afm, DateTime.Parse(fromdate), DateTime.Parse(nextdate));

            Phone phone = await getPhone(client.PhoneNumber);
            if(phone==null)
            {
                TempData["ErrorMessage"] = "The client does not have a phone yet.";
                return RedirectToAction("IssueBill", "Seller");
            }

            Package package = await getPackage(phone.Package);

            return View(new CallClientViewModel() { Client = client, Calls = calls, Package= package});
        }

        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> ChangeProgram(string afm, string package)
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

            Seller seller = await getSeller(userId);
            if (seller == null)
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index", "Home");
            }

            var packages = await _context.Packages.ToListAsync();

            if (string.IsNullOrEmpty(afm) || string.IsNullOrEmpty(package))
            {
                return View(packages);
            }
            Client client = await getClient(afm);
            if(client==null)
            {
                TempData["ErrorMessage"] = "The client was not found.";
                return View(packages);
            }

            if(string.IsNullOrEmpty(client.PhoneNumber))
            {
                TempData["ErrorMessage"] = "The client does not have a phone yet.";
                return View(packages);
            }

            Phone phone = await getPhone(client.PhoneNumber);
            if (phone == null)
            {
                TempData["ErrorMessage"] = "The client does not have a phone yet.";
                return View(packages);
            }
            phone.Package = package;
            _context.SaveChanges();
            TempData["SuccessMessage"] = "All changes were successfully saved.";
            return View(packages);
        }


        [HttpPost]
        public async Task<IActionResult> RegisterClient(string afm, string existingclient, string firstname, string lastname, string username,
            string password, string phone, string package, string renewdate)
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

            Seller seller = await getSeller(userId);
            if (seller == null)
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index", "Home");
            }


            if(string.IsNullOrEmpty(existingclient))
            {
                Client client = await getClient(afm);
                if(client == null)
                {
                    TempData["ErrorMessage"] = "The client was not found.";
                    return RedirectToAction("AddClient", "Seller");
                }

                if(string.IsNullOrEmpty(phone))
                {
                    if(string.IsNullOrEmpty(client.PhoneNumber))
                    {
                        TempData["ErrorMessage"] = "The client does not have a phone number.";
                        return RedirectToAction("AddClient", "Seller");
                    }

                    Phone phone_object = await getPhone(client.PhoneNumber);
                    if(phone_object==null)
                    {

                        TempData["ErrorMessage"] = "The client does not have a phone number.";
                        return RedirectToAction("AddClient", "Seller");
                    }

                    if (!string.IsNullOrEmpty(package)) phone_object.Package = package;
                    if (!string.IsNullOrEmpty(renewdate)) phone_object.renewDate = DateTime.Parse(renewdate);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "All changes were successfully saved.";
                    return RedirectToAction("AddClient", "Seller");
                } else
                {

                    if(await getPhone(phone) != null)
                    {
                        TempData["ErrorMessage"] = "The phone number specified already exists.";
                        return RedirectToAction("AddClient", "Seller");

                    }


                    Phone phone_object = await getPhone(client.PhoneNumber);
                    if (phone_object != null) _context.Phones.Remove(phone_object);


                    Phone newPhone = new Phone { 
                        Number = phone, 
                        issuedDate = DateTime.Now, 
                        renewDate = (string.IsNullOrEmpty(renewdate)) ? DateTime.Now.AddMonths(1) : DateTime.Parse(renewdate),
                        Package = package
                    };
                    client.PhoneNumber = phone;

                    await _context.Phones.AddAsync(newPhone);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "All changes were successfully saved.";
                    return RedirectToAction("AddClient", "Seller");
                }


            } else
            {

                if (await getClient(afm) != null)
                {
                    TempData["ErrorMessage"] = "This AFM is already taken.";
                    return RedirectToAction("AddClient", "Seller");

                }

                if (await getClientByUsername(username) != null)
                {
                    TempData["ErrorMessage"] = "This username is already taken.";
                    return RedirectToAction("AddClient", "Seller");

                }

                if(string.IsNullOrEmpty(phone) ||
                    string.IsNullOrEmpty(package))
                {
                    TempData["ErrorMessage"] = "Phone/Package was not specified.";
                    return RedirectToAction("AddClient", "Seller");
                }

                Phone phone_object = await getPhone(phone);
                if(phone_object != null)
                {
                    TempData["ErrorMessage"] = "The phone number specified already exists.";
                    return RedirectToAction("AddClient", "Seller");
                }

                Phone newPhone = new Phone
                {
                    Number = phone,
                    issuedDate = DateTime.Now,
                    renewDate = (string.IsNullOrEmpty(renewdate)) ? DateTime.Now.AddMonths(1) : DateTime.Parse(renewdate),
                    Package = package
                };


                Client client = new Client
                {
                    FirstName = firstname,
                    LastName = lastname,
                    Username = username,
                    Password = password,
                    AFM = afm,
                    PhoneNumber = phone
                };



                _context.Phones.Add(newPhone);
                _context.Clients.Add(client);

                _context.SaveChanges();
                TempData["SuccessMessage"] = "All changes were successfully saved.";
                return RedirectToAction("AddClient", "Seller");
            }
        }

        private async Task<Seller> getSeller(int userId) => await _context.Sellers.FirstOrDefaultAsync(s => s.Id == userId);
        private async Task<Client> getClient(string afm) => await _context.Clients.FirstOrDefaultAsync(u => u.AFM == afm);
        private async Task<Client> getClientByUsername(string username) => await _context.Clients.FirstOrDefaultAsync(u => u.Username == username);
        private async Task<Phone> getPhone(string phone) => await _context.Phones.FirstOrDefaultAsync(p => p.Number == phone);
        private async Task<List<Call>> getCalls(string phone, DateTime fromDate, DateTime nextDate) => await _context.Calls.Where(c => c.PhoneSender == phone 
        && (c.Started >= fromDate && c.Started <= nextDate) && (c.Ended >= fromDate && c.Ended <= nextDate)
        ).ToListAsync();
        private async Task<Package> getPackage(string package) => await _context.Packages.FirstOrDefaultAsync(p => p.Name == package);

    }
}
