using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VulnerableApp.Models;

namespace VulnerableApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {
            var users = context.Users.ToList();
            return View(users);
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                ?? HttpContext.TraceIdentifier
            });
        }
    }
}