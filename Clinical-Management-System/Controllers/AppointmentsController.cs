using Clinical_Management_System.Data;
using Clinical_Management_System.Models.DB_Entities;
using Clinical_Management_System.Repository.IRepositery;
using Clinical_Management_System.Utitlity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize]
public class AppointmentsController : Controller
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ApplicationDbContext _context;

	public AppointmentsController(IUnitOfWork unitOfWork, ApplicationDbContext context)
	{
		_unitOfWork = unitOfWork;
		_context = context;
	}

	public IActionResult Index()
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		if (User.IsInRole(Sd.Role_Doctor))
		{
			var doctorAppointments = _unitOfWork.appointmentRepository
				.GetAll(a => a.DoctorId == userId, "Patient", "Schedule", "Clinic");
			return View(doctorAppointments);
		}
		else if (User.IsInRole(Sd.Role_Patient))
		{
			var patientAppointments = _unitOfWork.appointmentRepository
					.GetAll(a => a.DoctorId == userId, "Doctor", "Schedule", "Clinic");
			return View(patientAppointments);
		}

		return Unauthorized();
	}

	[Authorize(policy: Sd.Role_Doctor)]
	public IActionResult UpdateAppointment(int id)
	{
		var appointment =  _unitOfWork.appointmentRepository.Get(c=>c.AppointementId==id);

		if (appointment == null)
		{
			return NotFound();
		}

		return View(appointment);
	}

	[HttpPost]
	[Authorize(Policy = Sd.Role_Doctor)]
	public IActionResult UpdateAppointment(Appointment appointment)
	{
		var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		var existingAppointment = _unitOfWork.appointmentRepository.
			Get(c => c.AppointementId == appointment.AppointementId && c.DoctorId == doctorId);

		if (existingAppointment != null)
		{
			existingAppointment.Status = appointment.Status;
			existingAppointment.Notes = appointment.Notes;
			 _unitOfWork.Save();
			return RedirectToAction("Index");
		}
		return NotFound();
	}

	[Authorize(policy: Sd.Role_Patient)]
	public IActionResult Create()
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var takenSchedules = _context.Appointments.Select(a => a.ScheduleId).ToList();

		ViewData["DoctorList"] = _context.Doctors.Select(c => new SelectListItem
		{
			Value = c.Id,
			Text = $"{c.FirstName} {c.LastName}"
		});

		return View();
	}


	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(Appointment appointment)
	{
		var takenSchedules = _context.Appointments.Select(a => a.ScheduleId).ToList();
		var availableSchedules = _context.Schedule
											  .Where(s => !takenSchedules.Contains(s.Id))
											  .ToList();
		if (ModelState.IsValid)
		{
			_context.Add(appointment);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		ViewData["DoctorList"] = _context.Doctors.Select(c=>new SelectListItem
		{
			Value = c.Id,
			Text=$"{c.FirstName} {c.LastName}"
		});
		return View(appointment);
	}

	[Authorize(policy: Sd.Role_Patient)]
	public async Task<IActionResult> CancelAppointment(int id)
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		var appointment = await _context.Appointments
			.FirstOrDefaultAsync(a => a.AppointementId == id && a.PatientId == userId);

		if (appointment == null)
		{
			return NotFound();
		}

		appointment.Status = "Cancelled";
		await _context.SaveChangesAsync();

		TempData["AppointmentCancelled"] = true;

		return RedirectToAction("Index");
	}


	[Authorize(policy: Sd.Role_Doctor)]
	public async Task<IActionResult> Delete(int? id)
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (id == null)
		{
			return NotFound();
		}
		var appointment = await _context.Appointments
			.Include(a => a.Doctor)
			.Include(a => a.Patient).Where(c=>c.DoctorId==userId)
			.FirstOrDefaultAsync(m => m.AppointementId == id);
		if (appointment == null)
		{
			return NotFound();
		}

		return View(appointment);
	}
	[HttpPost]
	[ValidateAntiForgeryToken]
	[HttpPost, ActionName("Delete")]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		var appointment = await _context.Appointments
			.FirstOrDefaultAsync(a => a.AppointementId == id && a.DoctorId == userId);

		if (appointment == null)
		{
			return NotFound();
		}

		_context.Remove(appointment);
		await _context.SaveChangesAsync();
		return RedirectToAction("Index");
	}

	[HttpGet]
	public IActionResult GetClinicsAndSchedules(string doctorId)
	{
		var clinics = _context.Clinics
			.Where(c => c.DoctorId == doctorId)
			.Select(c => new { c.ClinicId, c.StreetName })
			.ToList();

		var takenSchedules = _context.Appointments.Select(a => a.ScheduleId).ToList();
		var schedules = _context.Schedule
			.Where(s => s.DoctorId == doctorId && !takenSchedules.Contains(s.Id))
			.Select(s => new { s.Id, s.AvailableDateTime })
			.ToList();

		return Json(new { clinics, schedules });
	}
}
