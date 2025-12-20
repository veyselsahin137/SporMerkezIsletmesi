using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SporMerkeziIsletmesi.Data;
using SporMerkeziIsletmesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporMerkeziIsletmesi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AntrenorMusaitlikController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AntrenorMusaitlikController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AntrenorMusaitlik
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AntrenorMusaitlikler.Include(a => a.Antrenor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AntrenorMusaitlik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenorMusaitlik = await _context.AntrenorMusaitlikler
                .Include(a => a.Antrenor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (antrenorMusaitlik == null)
            {
                return NotFound();
            }

            return View(antrenorMusaitlik);
        }

        // GET: AntrenorMusaitlik/Create
        public IActionResult Create()
        {
            ViewData["AntrenorID"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad");
            return View();
        }

        // POST: AntrenorMusaitlik/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AntrenorID,Gun,BaslangicSaati,BitisSaati")] AntrenorMusaitlik antrenorMusaitlik)
        {
            //  Önce çakışma var mı kontrol et
            var cakisiyorMu = _context.AntrenorMusaitlikler
                .Any(m =>
                    m.AntrenorID == antrenorMusaitlik.AntrenorID &&
                    m.Gun == antrenorMusaitlik.Gun &&
                    // Saat aralığı çakışma kontrolü:
                    m.BaslangicSaati < antrenorMusaitlik.BitisSaati &&
                    antrenorMusaitlik.BaslangicSaati < m.BitisSaati
                );

            if (cakisiyorMu)
            {
                ModelState.AddModelError(string.Empty, "Bu antrenör için bu gün ve saat aralığında zaten bir müsaitlik kaydı var (çakışma).");

                ViewData["AntrenorID"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad", antrenorMusaitlik.AntrenorID);
                return View(antrenorMusaitlik);
            }

            //  Model diğer validasyonlardan da geçtiyse kaydet
            if (ModelState.IsValid)
            {
                _context.Add(antrenorMusaitlik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AntrenorID"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad", antrenorMusaitlik.AntrenorID);
            return View(antrenorMusaitlik);
        }


        // GET: AntrenorMusaitlik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenorMusaitlik = await _context.AntrenorMusaitlikler.FindAsync(id);
            if (antrenorMusaitlik == null)
            {
                return NotFound();
            }
            ViewData["AntrenorID"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad", antrenorMusaitlik.AntrenorID);
            return View(antrenorMusaitlik);
        }

        // POST: AntrenorMusaitlik/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AntrenorID,Gun,BaslangicSaati,BitisSaati")] AntrenorMusaitlik antrenorMusaitlik)
        {
            if (id != antrenorMusaitlik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(antrenorMusaitlik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AntrenorMusaitlikExists(antrenorMusaitlik.Id))
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
            ViewData["AntrenorID"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad", antrenorMusaitlik.AntrenorID);
            return View(antrenorMusaitlik);
        }

        // GET: AntrenorMusaitlik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenorMusaitlik = await _context.AntrenorMusaitlikler
                .Include(a => a.Antrenor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (antrenorMusaitlik == null)
            {
                return NotFound();
            }

            return View(antrenorMusaitlik);
        }

        // POST: AntrenorMusaitlik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var antrenorMusaitlik = await _context.AntrenorMusaitlikler.FindAsync(id);
            if (antrenorMusaitlik != null)
            {
                _context.AntrenorMusaitlikler.Remove(antrenorMusaitlik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AntrenorMusaitlikExists(int id)
        {
            return _context.AntrenorMusaitlikler.Any(e => e.Id == id);
        }
    }
}
