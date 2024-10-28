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
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.AspNetCore.Authorization;
using Clinical_Management_System.Utitlity;

namespace Clinical_Management_System.Controllers
{
	[Authorize]
	public class AllergiesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public AllergiesController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.Allergies.Include(a => a.Patient);
			return View(await applicationDbContext.ToListAsync());
		}

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

		[Authorize(Policy = Sd.Role_Patient)]
		public IActionResult Create()
		{
			return View();
		}

		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name,PatientId")] Allergy allergy)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				return RedirectToAction("Account", "Login");
			}
			allergy.PatientId = userId;
			if (ModelState.IsValid)
			{
				_context.Add(allergy);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(allergy);
		}

		[Authorize(Policy = Sd.Role_Patient)]
		public async Task<IActionResult> Delete(string id)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (id == null)
			{
				return NotFound();
			}

			var allergy = await _context.Allergies
				.Include(a => a.Patient).Where(c => c.PatientId == userId)
				.FirstOrDefaultAsync(m => m.PatientId == id);
			if (allergy == null)
			{
				return NotFound();
			}

			return View(allergy);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var allergy = await _context.Allergies.Where(c=>c.PatientId==userId).FirstOrDefaultAsync(c=>c.PatientId == id);
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
