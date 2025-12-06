using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Randevu.Include(r => r.Antrenor).Include(r => r.Hizmet).Include(r => r.Uye);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Randevu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevu
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .Include(r => r.Uye)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (randevu == null)
            {
                return NotFound();
            }

            return View(randevu);
        }

        // GET: Randevu/Create
        public IActionResult Create()
        {
            ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad");
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi");
            ViewData["UyeId"] = new SelectList(_context.Uyeler, "Id", "KullaniciId");
            return View();
        }

        // POST: Randevu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UyeId,AntrenorId,HizmetId,Tarih,Durum")] Randevu randevu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(randevu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad", randevu.AntrenorId);
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi", randevu.HizmetId);
            ViewData["UyeId"] = new SelectList(_context.Uyeler, "Id", "KullaniciId", randevu.UyeId);
            return View(randevu);
        }

        // GET: Randevu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevu.FindAsync(id);
            if (randevu == null)
            {
                return NotFound();
            }
            ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad", randevu.AntrenorId);
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi", randevu.HizmetId);
            ViewData["UyeId"] = new SelectList(_context.Uyeler, "Id", "KullaniciId", randevu.UyeId);
            return View(randevu);
        }

        // POST: Randevu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UyeId,AntrenorId,HizmetId,Tarih,Durum")] Randevu randevu)
        {
            if (id != randevu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(randevu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RandevuExists(randevu.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad", randevu.AntrenorId);
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi", randevu.HizmetId);
            ViewData["UyeId"] = new SelectList(_context.Uyeler, "Id", "KullaniciId", randevu.UyeId);
            return View(randevu);
        }

        // GET: Randevu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevu
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .Include(r => r.Uye)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (randevu == null)
            {
                return NotFound();
            }

            return View(randevu);
        }

        // POST: Randevu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var randevu = await _context.Randevu.FindAsync(id);
            if (randevu != null)
            {
                _context.Randevu.Remove(randevu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RandevuExists(int id)
        {
            return _context.Randevu.Any(e => e.Id == id);
        }
    }
}
