﻿using Clinical_Management_System.Data;
using Clinical_Management_System.Models.DB_Entities;
using Clinical_Management_System.Utitlity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize]
public class AppointmentsController : Controller
{
	private readonly ApplicationDbContext _context;

	public AppointmentsController(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IActionResult> Index()
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		if (User.IsInRole(Sd.Role_Doctor))
		{
			var doctorAppointments = await _context.Appointments
				.Where(a => a.DoctorId == userId)
				.Include(a => a.Patient)
				.Include(a => a.Schedule)
				.ToListAsync();

			return View(doctorAppointments);
		}
		else if (User.IsInRole(Sd.Role_Patient))
		{
			var patientAppointments = await _context.Appointments
				.Where(a => a.PatientId == userId)
				.Include(a => a.Doctor).ThenInclude(a=>a.Specialization)
				.Include(a => a.Schedule)
				.ToListAsync();

			return View(patientAppointments);
		}

		return Unauthorized();
	}

	[Authorize(policy: Sd.Role_Doctor)]
	public async Task<IActionResult> UpdateAppointment(int id)
	{
		var appointment = await _context.Appointments.FindAsync(id);

		if (appointment == null)
		{
			return NotFound();
		}

		return View(appointment);
	}

	[HttpPost]
	[Authorize(Policy = Sd.Role_Doctor)]
	public async Task<IActionResult> UpdateAppointment(Appointment appointment)
	{
		var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		var existingAppointment = await _context.Appointments
			.FirstOrDefaultAsync(a => a.AppointementId == appointment.AppointementId && a.DoctorId == doctorId);

		if (existingAppointment != null)
		{
			existingAppointment.Status = appointment.Status;
			existingAppointment.Notes = appointment.Notes;
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		return NotFound();
	}

	[Authorize(policy: Sd.Role_Patient)]
	public IActionResult Create()
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

		var takenSchedules = _context.Appointments.Select(a => a.ScheduleId).ToList();
		if (User.IsInRole("Patient"))
		{
			var availableSchedules = _context.Schedule
											  .Where(s => !takenSchedules.Contains(s.Id))
											  .ToList();

			ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "UserName");
			ViewData["ScheduleId"] = new SelectList(availableSchedules, "Id", "AvailableDateTime");
		}
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	[Authorize(policy: Sd.Role_Patient)]
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
		ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "UserName");
		ViewData["ScheduleId"] = new SelectList(availableSchedules, "Id", "AvailableDateTime");
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
		return RedirectToAction("Index");
	}

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
	[Authorize(policy: Sd.Role_Doctor)]
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
}
