using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinical_Management_System.Data;
using Clinical_Management_System.Models.DB_Entities;
using Clinical_Management_System.ViewModel;
using System.Security.Claims;

namespace Clinical_Management_System.Controllers
{
	public class PrescriptionsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public PrescriptionsController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationDbContext = _context.Prescriptions.Where(P => P.Appointment.PatientId == userId || P.Appointment.DoctorId == userId).Include(p => p.Appointment).ThenInclude(d=>d.Doctor);
            return View(await applicationDbContext.ToListAsync());
        }

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var prescription = await _context.Prescriptions
				.Include(p => p.Appointment)
				.FirstOrDefaultAsync(m => m.PrescriptionId == id);
			if (prescription == null)
			{
				return NotFound();
			}

			return View(prescription);
		}

		public IActionResult Create()
		{
			var takenAppointmentIds = _context.Prescriptions.Select(p => p.AppointmentId).ToList();
			PrescriptionVM prescriptionVM = new()
			{
				Appointments = _context.Appointments
				.Where(a => !takenAppointmentIds.Contains(a.AppointementId)) // Exclude taken appointment IDs
				.Select(a => new SelectListItem
				{
					Value = a.AppointementId.ToString(),
					Text = a.Reason
				}).ToList(),

			};

			return View(prescriptionVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(PrescriptionVM prescriptionVM)
		{
			if (ModelState.IsValid)
			{
				var prescription = new Prescription
				{
					PrescriptionId = prescriptionVM.PrescriptionId,
					AppointmentId = prescriptionVM.AppointmentId,
					DateTime = prescriptionVM.DateTime,
					DiagnosisName = prescriptionVM.DiagnosisName,
				};

				_context.Add(prescription);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			var takenAppointmentIds = await _context.Prescriptions
				.Select(p => p.AppointmentId)
				.ToListAsync();

			prescriptionVM.Appointments = await _context.Appointments
				.Where(a => !takenAppointmentIds.Contains(a.AppointementId))
				.Select(a => new SelectListItem
				{
					Value = a.AppointementId.ToString(),
					Text = a.Reason,

				}).ToListAsync();

			return View(prescriptionVM);
		}



		public async Task<IActionResult> Edit(int? id)
		{

			if (id == null)
			{
				return NotFound();
			}
			var prescription = await _context.Prescriptions.FindAsync(id);
			if (prescription == null)
			{
				return NotFound();
			}
			var takenAppointmentIds = _context.Prescriptions.Select(p => p.AppointmentId).ToList();
			PrescriptionVM prescriptionVM = new()
			{

				PrescriptionId = prescription.PrescriptionId,
				DiagnosisName = prescription.DiagnosisName,
				DateTime = prescription.DateTime,
				AppointmentId = prescription.AppointmentId,
				Appointments = _context.Appointments
				.Where(a => !takenAppointmentIds.Contains(a.AppointementId))
				.Select(a => new SelectListItem
				{
					Value = a.AppointementId.ToString(),
				}).ToList(),

			};

			return View(prescriptionVM);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, PrescriptionVM prescriptionVM)
		{
			if (id != prescriptionVM.PrescriptionId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					var prescription = await _context.Prescriptions.FindAsync(id);
					if (prescription == null)
					{
						return NotFound();
					}

					// Update prescription details
					prescription.AppointmentId = prescriptionVM.AppointmentId;
					prescription.DateTime = prescriptionVM.DateTime;
					prescription.DiagnosisName = prescriptionVM.DiagnosisName;

					_context.Update(prescription); // Update the prescription in the context
					await _context.SaveChangesAsync(); // Save changes
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PrescriptionExists(prescriptionVM.PrescriptionId))
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

			// Repopulate the view model if model state is invalid
			var takenAppointmentIds = _context.Prescriptions.Select(p => p.AppointmentId).ToList();
			prescriptionVM.Appointments = _context.Appointments
				.Where(a => !takenAppointmentIds.Contains(a.AppointementId))
				.Select(a => new SelectListItem
				{
					Value = a.AppointementId.ToString(),
				}).ToList();

			return View(prescriptionVM);
		}


		// GET: Prescriptions/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var prescription = await _context.Prescriptions
				.Include(p => p.Appointment)
				.FirstOrDefaultAsync(m => m.PrescriptionId == id);
			if (prescription == null)
			{
				return NotFound();
			}

			return View(prescription);
		}

		// POST: Prescriptions/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var prescription = await _context.Prescriptions.FindAsync(id);
			if (prescription != null)
			{
				_context.Prescriptions.Remove(prescription);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool PrescriptionExists(int id)
		{
			return _context.Prescriptions.Any(e => e.PrescriptionId == id);
		}
	}
}
