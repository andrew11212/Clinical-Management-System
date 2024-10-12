using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinical_Management_System.Data;
using Clinical_Management_System.Models.DB_Entities;

namespace Clinical_Management_System.Controllers
{
    public class AllergiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AllergiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Allergies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Allergies.Include(a => a.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Allergies/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allergy = await _context.Allergies
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (allergy == null)
            {
                return NotFound();
            }

            return View(allergy);
        }

        // GET: Allergies/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id");
            return View();
        }

        // POST: Allergies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,PatientId")] Allergy allergy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allergy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", allergy.PatientId);
            return View(allergy);
        }

        // GET: Allergies/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allergy = await _context.Allergies.FindAsync(id);
            if (allergy == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", allergy.PatientId);
            return View(allergy);
        }

        // POST: Allergies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,PatientId")] Allergy allergy)
        {
            if (id != allergy.PatientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allergy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllergyExists(allergy.PatientId))
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
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", allergy.PatientId);
            return View(allergy);
        }

        // GET: Allergies/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allergy = await _context.Allergies
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (allergy == null)
            {
                return NotFound();
            }

            return View(allergy);
        }

        // POST: Allergies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var allergy = await _context.Allergies.FindAsync(id);
            if (allergy != null)
            {
                _context.Allergies.Remove(allergy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AllergyExists(string id)
        {
            return _context.Allergies.Any(e => e.PatientId == id);
        }
    }
}
