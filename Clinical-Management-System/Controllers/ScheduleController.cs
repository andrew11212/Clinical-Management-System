using Clinical_Management_System.Data;
using Clinical_Management_System.Models.DB_Entities;
using Clinical_Management_System.Utitlity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Clinical_Management_System.Controllers
{
	[Authorize(Policy = Sd.Role_Doctor)]
	public class ScheduleController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ScheduleController(ApplicationDbContext context)
		{
			_context = context;
		} 

		// GET: Schedule
		// GET: Schedule/Index
		public async Task<IActionResult> Index()
		{
			var claimIdentity = User.Identity as ClaimsIdentity;
			var userId = claimIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				return RedirectToAction("Login", "Account"); 
			}

			var schedules = await _context.Schedule
				.Where(s => s.DoctorId == userId).Include(d => d.Doctor) 
				.ToListAsync();

			return View(schedules); 
		}


		// GET: Schedule/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Schedule/Create
		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> Create(Schedule schedule)
		{
			var claimIdentity = User.Identity as ClaimsIdentity;
			var userId = claimIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			// Check if the user is authenticated and set the DoctorId
			if (userId != null)
			{
				schedule.DoctorId = userId; 
			}

			if (ModelState.IsValid)
			{
				await _context.Schedule.AddAsync(schedule);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(schedule);
		}

		// GET: Schedule/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			var schedule = await _context.Schedule.FindAsync(id);
			if (schedule == null)
			{
				return NotFound();
			}
			return View(schedule);
		}

		// POST: Schedule/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Schedule schedule)
		{
			if (id != schedule.Id)
			{
				return NotFound();
			}

			var claimIdentity = User.Identity as ClaimsIdentity;
			var userId = claimIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			// Check if the user is authenticated and set the DoctorId
			if (userId != null)
			{
				schedule.DoctorId = userId; 
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Schedule.Update(schedule); 
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ScheduleExists(schedule.Id))
					{
						return NotFound();
					}
					throw; 
				}
				return RedirectToAction(nameof(Index));
			}
			return View(schedule);
		}


		// GET: Schedule/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var schedule = await _context.Schedule
										  .Include(s => s.Doctor) // Include doctor details if needed
										  .FirstOrDefaultAsync(m => m.Id == id);
			if (schedule == null)
			{
				return NotFound();
			}
			return View(schedule);
		}

		// POST: Schedule/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var schedule = await _context.Schedule.FindAsync(id);
			if (schedule != null)
			{
				_context.Schedule.Remove(schedule);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}

		private bool ScheduleExists(int id)
		{
			return _context.Schedule.Any(e => e.Id == id);
		}
		[HttpGet]
		public IActionResult GetSchedulesByClinic(int clinicId)
		{
			var doctorId = _context.Clinics
				.Where(c => c.ClinicId == clinicId)
				.Select(c => c.DoctorId).FirstOrDefault();

			if (doctorId == null)
			{
				return Json(new { success = false, message = "Doctor not found for the selected clinic." });
			}
			var schedules = _context.Schedule
			.Where(s => s.DoctorId == doctorId)
			.Select(s => new
			{
				s.Id,
				AvailableDateTime = s.AvailableDateTime.ToString("yyyy-MM-dd HH:mm")
			}).ToList();
			return Json(schedules);

		}
	}
}
