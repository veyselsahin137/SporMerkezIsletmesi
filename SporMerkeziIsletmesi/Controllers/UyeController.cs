using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SporMerkeziIsletmesi.Data;
using SporMerkeziIsletmesi.Models;

namespace SporMerkeziIsletmesi.Controllers
{
    [Authorize]
    public class UyeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UyeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // --------------------------------------------------------------
        // 1) Üye Profilim sayfası (varsa düzenleme, yoksa oluşturma)
        // --------------------------------------------------------------
        public async Task<IActionResult> Profilim()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var uye = await _context.Uyeler
                .Include(x => x.Salon)
                .FirstOrDefaultAsync(u => u.KullaniciId == user.Id);

            if (uye == null)
                return RedirectToAction("Create");

            return View("Edit", uye);
        }

        // --------------------------------------------------------------
        // 2) Yeni Üye Profili Oluşturma (Register sonrası ilk girişte)
        // --------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);

            // Zaten profili varsa direkt düzenlemeye at
            var mevcut = await _context.Uyeler.FirstOrDefaultAsync(u => u.KullaniciId == user.Id);
            if (mevcut != null)
                return RedirectToAction("Edit", new { id = mevcut.Id });

            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Uye uye)
        {
            var user = await _userManager.GetUserAsync(User);

            // Identity User ID eşleştirmesi
            uye.KullaniciId = user.Id;

            if (ModelState.IsValid)
            {
                _context.Add(uye);
                await _context.SaveChangesAsync();
                return RedirectToAction("Profilim");
            }

            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad", uye.SalonId);
            return View(uye);
        }

        // --------------------------------------------------------------
        // 3) Üye Bilgileri Güncelleme
        // --------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var uye = await _context.Uyeler.FindAsync(id);
            if (uye == null)
                return NotFound();

            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad", uye.SalonId);
            return View(uye);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Uye uye)
        {
            if (id != uye.Id)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            uye.KullaniciId = user.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uye);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Uyeler.Any(e => e.Id == uye.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Profilim");
            }

            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad", uye.SalonId);
            return View(uye);
        }
    }
}
