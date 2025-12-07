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
    public class SalonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Salon
        public async Task<IActionResult> Index()
        {
            return View(await _context.Salonlar.ToListAsync());
        }

        // GET: Salon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salon = await _context.Salonlar
                .FirstOrDefaultAsync(m => m.SalonID == id);
            if (salon == null)
            {
                return NotFound();
            }

            return View(salon);
        }

        // GET: Salon/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Salon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalonID,SalonAdi,Adres,AcilisSaati,KapanisSaati")] Salon salon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salon);
        }

        // GET: Salon/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salon = await _context.Salonlar.FindAsync(id);
            if (salon == null)
            {
                return NotFound();
            }
            return View(salon);
        }

        // POST: Salon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalonID,SalonAdi,Adres,AcilisSaati,KapanisSaati")] Salon salon)
        {
            if (id != salon.SalonID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalonExists(salon.SalonID))
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
            return View(salon);
        }

        // GET: Salon/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salon = await _context.Salonlar
                .FirstOrDefaultAsync(m => m.SalonID == id);
            if (salon == null)
            {
                return NotFound();
            }

            return View(salon);
        }

        // POST: Salon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salon = await _context.Salonlar.FindAsync(id);
            if (salon != null)
            {
                _context.Salonlar.Remove(salon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalonExists(int id)
        {
            return _context.Salonlar.Any(e => e.SalonID == id);
        }
    }
}
