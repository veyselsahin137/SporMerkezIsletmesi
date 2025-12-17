using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SporMerkeziIsletmesi.Data;
using SporMerkeziIsletmesi.Models;

namespace SporMerkeziIsletmesi.Controllers
{
    public class RandevuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RandevuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Randevu
        public IActionResult Index()
        {
            var randevular = _context.Randevular
                .Include(r => r.Uye)
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .ToList();

            return View(randevular);
        }

        // GET: Randevu/Create
        public IActionResult Create()
        {
            ViewBag.Uyeler = new SelectList(_context.Uyeler, "Id", "Ad");
            ViewBag.Antrenorler = new SelectList(_context.Antrenorler, "AntrenorID", "Ad");
            ViewBag.Hizmetler = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi");

            return View();
        }


        // POST: Randevu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Randevu randevu)
        {
            if (ModelState.IsValid)
            {
                _context.Randevular.Add(randevu);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Uyeler = _context.Uyeler.ToList();
            ViewBag.Antrenorler = _context.Antrenorler.ToList();
            ViewBag.Hizmetler = _context.Hizmetler.ToList();

            return View(randevu);
        }
    }
}
