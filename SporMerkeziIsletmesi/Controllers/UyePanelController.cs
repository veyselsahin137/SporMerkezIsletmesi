using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporMerkeziIsletmesi.Data;
using SporMerkeziIsletmesi.Models;

namespace SporMerkeziIsletmesi.Controllers
{
    [Authorize]
    public class UyePanelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UyePanelController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index() => View();

        public IActionResult Randevular() => View();

        // ===============================
        // RANDEVU OLUŞTUR (GET)
        // ===============================
        public IActionResult RandevuOlustur()
        {
            ViewBag.Hizmetler = _context.Hizmetler
                .Include(h => h.Salon)
                .ToList();

            return View();
        }

        // ===============================
        // RANDEVU OLUŞTUR (POST)
        // ===============================
        [HttpPost]
        public async Task<IActionResult> RandevuOlustur(int AntrenorID, int HizmetID, string SecilenSlot)
        {
            var user = await _userManager.GetUserAsync(User);
            var uye = await _context.Uyeler.FirstAsync(u => u.IdentityUserId == user.Id);

            var parts = SecilenSlot.Split('|');

            var tarih = DateTime.Parse(parts[0]);
            var bas = TimeSpan.Parse(parts[1]);
            var bit = TimeSpan.Parse(parts[2]);

            var randevu = new Randevu
            {
                UyeId = uye.Id,
                AntrenorID = AntrenorID,
                HizmetID = HizmetID,
                Tarih = tarih,
                BaslangicSaati = bas,
                BitisSaati = bit,
                Durum = RandevuDurum.Beklemede,
                OlusturmaTarihi = DateTime.Now
            };

            _context.Randevular.Add(randevu);
            await _context.SaveChangesAsync();

            return RedirectToAction("Randevular");
        }

        // ===============================
        // PROFİL
        // ===============================
        public async Task<IActionResult> Profil()
        {
            var user = await _userManager.GetUserAsync(User);
            var uye = await _context.Uyeler.FirstOrDefaultAsync(u => u.IdentityUserId == user.Id);

            if (uye == null) return NotFound();
            return View(uye);
        }

        [HttpPost]
        public async Task<IActionResult> ProfilGuncelle(int? boy, int? kilo)
        {
            var user = await _userManager.GetUserAsync(User);
            var uye = await _context.Uyeler.FirstAsync(u => u.IdentityUserId == user.Id);

            uye.Boy = boy;
            uye.Kilo = kilo;

            await _context.SaveChangesAsync();
            return RedirectToAction("Profil");
        }
    }
}
