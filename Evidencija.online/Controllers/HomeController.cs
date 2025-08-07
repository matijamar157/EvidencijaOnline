using Evidencija.online.Data;
using Evidencija.online.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Evidencija.online.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.Name != null)
            {
                Korisnik korisnik = _context.Korisnik.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

                if (korisnik == null)
                {
                    return RedirectToAction("Create", "Korisnik");
                }
                else
                {
                    return RedirectToAction("Index", "Trosak");
                }
            }

            return View();
        }

        public IActionResult ActivatePremiumStripe()
        {
            return Redirect("https://buy.stripe.com/fZe6rPeRe2Z6fokg0v");
        }

        public IActionResult ActivateGoldStripe()
        {
            return Redirect("https://buy.stripe.com/7sI4jH8sQbvC2ByaGc");
        }

        public IActionResult BuyPackage()
        {
            return View();
        }

        public IActionResult Packages()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
