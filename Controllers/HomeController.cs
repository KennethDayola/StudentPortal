using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;
using System.Diagnostics;
using System.Security.Claims;

namespace StudentPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User viewModel)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await dbContext.Users
                    .FirstOrDefaultAsync(u => u.Username == viewModel.Username && u.Password == viewModel.Password);

                if (existingUser != null)
                {
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, existingUser.Username),
                            new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString())
                        };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    ViewBag.AlertMessage = "Logged in successfully! Redirecting to Home Page...";
                    ModelState.Clear();
                    return View();
                }

                ViewBag.AlertMessage = "Invalid Username or Password."; 
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup (User viewModel)
        {
            var existingUsername = await dbContext.Users
                   .FirstOrDefaultAsync(u => u.Username == viewModel.Username);
            if (existingUsername != null)
            {
                ViewBag.AlertMessage = "Username already exists!";
                return View(viewModel);
            }
            
            var user = new User { 
                Username = viewModel.Username,
                Password = viewModel.Password 
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            ModelState.Clear();
            ViewBag.AlertMessage = "Account successfully registered! Redirecting to Login Page...";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            TempData["UnauthorizedMessage"] = "To access other subpages, you need to be registered and logged in.";
            return RedirectToAction("Signup", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
