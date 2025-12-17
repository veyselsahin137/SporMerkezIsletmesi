using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporMerkeziIsletmesi.Data;
using SporMerkeziIsletmesi.Models;

namespace SporMerkeziIsletmesi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UyeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UyeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Uye
        public async Task<IActionResult> Index()
        {
            var uyeler = await _context.Uyeler.ToListAsync();
            return View(uyeler);
        }

        // GET: /Uye/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Uye/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Uye uye)
        {
            if (!ModelState.IsValid)
                return View(uye);

            _context.Uyeler.Add(uye);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
