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
		[Authorize(Policy = Sd.Role_Patient)]
		public async Task<IActionResult> Details(string id)
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
		[Authorize(Policy = Sd.Role_Patient)]
		public IActionResult Create()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = Sd.Role_Patient)]
		public async Task<IActionResult> Create([Bind("Name,PatientId")] ChronicDisease chronicDisease)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				return RedirectToAction("Account", "Login");
			}
			chronicDisease.PatientId = userId;
			if (ModelState.IsValid)
			{
				_context.Add(chronicDisease);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(chronicDisease);
		}

		

		[Authorize(Policy = Sd.Role_Patient)]
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var chronicDisease = await _context.ChronicDiseases
				.Include(c => c.Patient)
				.Where(m => m.PatientId == userId).FirstOrDefaultAsync(m => m.PatientId == id);
			if (chronicDisease == null)
			{
				return NotFound();
			}

			return View(chronicDisease);
		}

		// POST: ChronicDiseases/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = Sd.Role_Patient)]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var chronicDisease = await _context.ChronicDiseases.Where(m => m.PatientId == userId).FirstOrDefaultAsync(m => m.PatientId == id);
			if (chronicDisease != null)
			{
				_context.ChronicDiseases.Remove(chronicDisease);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ChronicDiseaseExists(string id)
		{
			return _context.ChronicDiseases.Any(e => e.PatientId == id);
		}
	}
}
