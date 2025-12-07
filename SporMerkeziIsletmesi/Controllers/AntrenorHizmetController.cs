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
    public class AntrenorHizmetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AntrenorHizmetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AntrenorHizmet
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AntrenorHizmetler.Include(a => a.Antrenor).Include(a => a.Hizmet);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AntrenorHizmet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenorHizmet = await _context.AntrenorHizmetler
                .Include(a => a.Antrenor)
                .Include(a => a.Hizmet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (antrenorHizmet == null)
            {
                return NotFound();
            }

            return View(antrenorHizmet);
        }

        // GET: AntrenorHizmet/Create
        public IActionResult Create()
        {
            ViewData["AntrenorID"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad");
            ViewData["HizmetID"] = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi");
            return View();
        }

        // POST: AntrenorHizmet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AntrenorID,HizmetID")] AntrenorHizmet antrenorHizmet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(antrenorHizmet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AntrenorID"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad", antrenorHizmet.AntrenorID);
            ViewData["HizmetID"] = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi", antrenorHizmet.HizmetID);
            return View(antrenorHizmet);
        }

        // GET: AntrenorHizmet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenorHizmet = await _context.AntrenorHizmetler.FindAsync(id);
            if (antrenorHizmet == null)
            {
                return NotFound();
            }
            ViewData["AntrenorID"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad", antrenorHizmet.AntrenorID);
            ViewData["HizmetID"] = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi", antrenorHizmet.HizmetID);
            return View(antrenorHizmet);
        }

        // POST: AntrenorHizmet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AntrenorID,HizmetID")] AntrenorHizmet antrenorHizmet)
        {
            if (id != antrenorHizmet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(antrenorHizmet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AntrenorHizmetExists(antrenorHizmet.Id))
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
            ViewData["AntrenorID"] = new SelectList(_context.Antrenorler, "AntrenorID", "Ad", antrenorHizmet.AntrenorID);
            ViewData["HizmetID"] = new SelectList(_context.Hizmetler, "HizmetID", "HizmetAdi", antrenorHizmet.HizmetID);
            return View(antrenorHizmet);
        }

        // GET: AntrenorHizmet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenorHizmet = await _context.AntrenorHizmetler
                .Include(a => a.Antrenor)
                .Include(a => a.Hizmet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (antrenorHizmet == null)
            {
                return NotFound();
            }

            return View(antrenorHizmet);
        }

        // POST: AntrenorHizmet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var antrenorHizmet = await _context.AntrenorHizmetler.FindAsync(id);
            if (antrenorHizmet != null)
            {
                _context.AntrenorHizmetler.Remove(antrenorHizmet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AntrenorHizmetExists(int id)
        {
            return _context.AntrenorHizmetler.Any(e => e.Id == id);
        }
    }
}
