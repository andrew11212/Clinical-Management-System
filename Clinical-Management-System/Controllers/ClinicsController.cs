using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinical_Management_System.Data;
using Clinical_Management_System.Models.DB_Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Clinical_Management_System.Utitlity;
using Clinical_Management_System.Repository.IRepositery;

namespace Clinical_Management_System.Controllers
{
	[Authorize(Policy = Sd.Role_Doctor)]
	public class ClinicsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IUnitOfWork _unitOfWork;

		public ClinicsController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var claimIdentity = User.Identity as ClaimsIdentity;
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Account");
			}
			var userId = claimIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				return RedirectToAction("Login", "Account");

			}
			var applicationDbContext = _context.Clinics
				.Where(c => c.DoctorId == userId)
				.Include(c => c.Doctor)
				.Include(c => c.Appointments)
				.ToListAsync();

			return View(await applicationDbContext);
		}

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

		public IActionResult Create()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Clinic clinic)
		{
			var claims = User.Identity as ClaimsIdentity;
			var Userid = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (Userid == null)
			{
				return RedirectToAction("Login", "Account");

			}
			clinic.DoctorId = Userid;
			if (ModelState.IsValid)
			{
				_context.Add(clinic);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(clinic);
		}

		// GET: Clinics/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			var claims = User.Identity as ClaimsIdentity;
			var userId = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (id == null)
			{
				return NotFound();
			}
			var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.ClinicId == id && c.DoctorId == userId);
			if (clinic == null)
			{
				return NotFound();
			}
			ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", clinic.DoctorId);
			return View(clinic);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Clinic clinic)
		{

			if (id != clinic.ClinicId)
			{
				return NotFound();
			}
			var claims = User.Identity as ClaimsIdentity;
			var Userid = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (Userid != null)
			{
				clinic.DoctorId = Userid;
			}
			if (ModelState.IsValid)
			{

				_context.Update(clinic);
				await _context.SaveChangesAsync();

				if (!ClinicExists(clinic.ClinicId))
				{
					return NotFound();
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "UserName", clinic.DoctorId);
			return View(clinic);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			var claimsIdentity = User.Identity as ClaimsIdentity;
			var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (id == null || userId == null)
			{
				return NotFound();
			}

			var clinic = await _context.Clinics
				.Include(c => c.Doctor)
				.FirstOrDefaultAsync(c => c.ClinicId == id && c.DoctorId == userId);

			if (clinic == null)
			{
				return NotFound();
			}

			return View(clinic);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var claimsIdentity = User.Identity as ClaimsIdentity;
			var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				return RedirectToAction("Login", "Account");
			}

			var clinic = await _context.Clinics
				.FirstOrDefaultAsync(c => c.ClinicId == id && c.DoctorId == userId);

			if (clinic != null)
			{
				_context.Clinics.Remove(clinic);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}


		private bool ClinicExists(int id)
		{
			return _context.Clinics.Any(e => e.ClinicId == id);
		}
	}
}
