using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinical_Management_System.Data;
using Clinical_Management_System.Models.DB_Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Clinical_Management_System.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        { 
            var applicationDbContext = _context.Appointments.Include(a => a.Clinic).Include(a => a.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Clinic)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AppointementId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {

           
            ViewData["ClinicId"] = new SelectList(_context.Clinics, "ClinicId", "City");
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id");
            return View();
        }

		// POST: Appointments/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Create([Bind("AppointementId,Date,Type,Reason,Notes,Hour,ClinicId,PatientId")] Appointment appointment)
		{
			// Get the user's claims identity
			var claims = User.Identity as ClaimsIdentity;
			var userId = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			// If the user ID is null, return unauthorized
			if (userId == null)
			{
				return Unauthorized();
			}

			// Set the PatientId from the logged-in user's ID
			appointment.PatientId = userId;

			// Check if the model state is valid
			if (ModelState.IsValid)
			{
				// Add the appointment to the context and save changes
				_context.Add(appointment);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));  // Redirect to the Index action on success
			}

			// Populate the clinic and patient selections for the view
			ViewData["ClinicId"] = new SelectList(_context.Clinics, "ClinicId", "City", appointment.ClinicId);
			ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", appointment.PatientId);

			// Return the view with the appointment model
			return View(appointment);
		}


		// GET: Appointments/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["ClinicId"] = new SelectList(_context.Clinics, "ClinicId", "City", appointment.ClinicId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", appointment.PatientId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointementId,Date,Type,Reason,Notes,Hour,ClinicId,PatientId")] Appointment appointment)
        {
            if (id != appointment.AppointementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointementId))
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
            ViewData["ClinicId"] = new SelectList(_context.Clinics, "ClinicId", "City", appointment.ClinicId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", appointment.PatientId);
            return View(appointment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Clinic)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AppointementId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointementId == id);
        }
    }
}
