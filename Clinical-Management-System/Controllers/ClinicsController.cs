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
    public class ClinicsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClinicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clinics
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Clinics.Include(c => c.Doctor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Clinics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _context.Clinics
                .Include(c => c.Doctor)
                .FirstOrDefaultAsync(m => m.ClinicId == id);
            if (clinic == null)
            {
                return NotFound();
            }

            return View(clinic);
        }

        // GET: Clinics/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id");
            return View();
        }

        // POST: Clinics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClinicId,OpenTime,CloseTime,City,BuildingNum,StreetName,Government,Floor,DoctorId")] Clinic clinic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clinic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", clinic.DoctorId);
            return View(clinic);
        }

        // GET: Clinics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _context.Clinics.FindAsync(id);
            if (clinic == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", clinic.DoctorId);
            return View(clinic);
        }

        // POST: Clinics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClinicId,OpenTime,CloseTime,City,BuildingNum,StreetName,Government,Floor,DoctorId")] Clinic clinic)
        {
            if (id != clinic.ClinicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clinic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClinicExists(clinic.ClinicId))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", clinic.DoctorId);
            return View(clinic);
        }

        // GET: Clinics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _context.Clinics
                .Include(c => c.Doctor)
                .FirstOrDefaultAsync(m => m.ClinicId == id);
            if (clinic == null)
            {
                return NotFound();
            }

            return View(clinic);
        }

        // POST: Clinics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clinic = await _context.Clinics.FindAsync(id);
            if (clinic != null)
            {
                _context.Clinics.Remove(clinic);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClinicExists(int id)
        {
            return _context.Clinics.Any(e => e.ClinicId == id);
        }
    }
}
