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
    public class ChronicDiseasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChronicDiseasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChronicDiseases
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ChronicDiseases.Include(c => c.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ChronicDiseases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chronicDisease = await _context.ChronicDiseases
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (chronicDisease == null)
            {
                return NotFound();
            }

            return View(chronicDisease);
        }

        // GET: ChronicDiseases/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "City");
            return View();
        }

        // POST: ChronicDiseases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,PatientId")] ChronicDisease chronicDisease)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chronicDisease);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "City", chronicDisease.PatientId);
            return View(chronicDisease);
        }

        // GET: ChronicDiseases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chronicDisease = await _context.ChronicDiseases.FindAsync(id);
            if (chronicDisease == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "City", chronicDisease.PatientId);
            return View(chronicDisease);
        }

        // POST: ChronicDiseases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,PatientId")] ChronicDisease chronicDisease)
        {
            if (id != chronicDisease.PatientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chronicDisease);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChronicDiseaseExists(chronicDisease.PatientId))
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
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "City", chronicDisease.PatientId);
            return View(chronicDisease);
        }

        // GET: ChronicDiseases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chronicDisease = await _context.ChronicDiseases
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (chronicDisease == null)
            {
                return NotFound();
            }

            return View(chronicDisease);
        }

        // POST: ChronicDiseases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chronicDisease = await _context.ChronicDiseases.FindAsync(id);
            if (chronicDisease != null)
            {
                _context.ChronicDiseases.Remove(chronicDisease);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChronicDiseaseExists(int id)
        {
            return _context.ChronicDiseases.Any(e => e.PatientId == id);
        }
    }
}
