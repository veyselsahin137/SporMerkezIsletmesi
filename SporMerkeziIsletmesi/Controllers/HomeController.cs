using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SporMerkeziIsletmesi.Models;
using SporMerkeziIsletmesi.Data;   

namespace SporMerkeziIsletmesi.Controllers
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // ? REST API demolarýný göstereceðimiz sayfa
        // Model olarak Hizmet listesini gönderiyoruz (dropdown için)
        public IActionResult RestApi()
        {
            var hizmetler = _context.Hizmetler
                                    .OrderBy(h => h.HizmetAdi)
                                    .ToList();

            return View(hizmetler);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
