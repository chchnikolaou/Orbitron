using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orbitron.Data;
using Orbitron.Models;
using Orbitron.Models.ViewModels;
using System.Security.Claims;

namespace Orbitron.Controllers
{
    public class UserController : Controller
    {

        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create([Bind("FirstName", "LastName", "Username", "Password")] User user)
        {
            if (ModelState.IsValid)
            {
                if (await UserExists(user.Username))
                {
                    TempData["ErrorMessage"] = "This username is already taken.";
                 
                    return RedirectToAction("Register", "Home");
                }


                Client client = new Client
                {
                    Id = user.Id,
                    AFM = string.Empty,
                    PhoneNumber = string.Empty,
                    Username = user.Username,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Property = user.Property
                };
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Your account has been created successfully.";
                return RedirectToAction("Login", "Home");
            }

            TempData["ErrorMessage"] = "Please fill all the required information to proceed.";
            return RedirectToAction("Register", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username", "Password")] LoginViewModel user)
        {
            if(User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You are already logged in.";
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                
                User data = await getUser(user.Username);
                if (data==null)
                {
                    TempData["ErrorMessage"] = "Invalid username or password.";
                    return RedirectToAction("Login", "Home");
                }


                if(!user.Password.Equals(data.Password))
                {
                    TempData["ErrorMessage"] = "Invalid username or password.";
                    return RedirectToAction("Login", "Home");
                }

                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier, data.Id.ToString()),
                new Claim(ClaimTypes.Name, data.Username),
                new Claim(ClaimTypes.GivenName, data.FirstName),
                new Claim(ClaimTypes.Surname, data.LastName),
                new Claim(ClaimTypes.Role, data.Property.ToString())
                };

                

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                TempData["SuccessMessage"] = "You have successfully logged in.";
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = "Invalid username or password.";
            return RedirectToAction("Login", "Home");

        }

        public async Task<IActionResult> Logout()
        {
            if(!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You are not logged in.";
                return RedirectToAction("Login", "Home");
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["SuccessMessage"] = "You have successfully logged out.";
            return RedirectToAction("Login", "Home");
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        private async Task<User> getUser(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }


    }
}
