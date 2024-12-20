﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinical_Management_System.Data;
using Clinical_Management_System.Models.DB_Entities;
using System.Security.Claims;

namespace Clinical_Management_System.Controllers
{
	public class MedicinesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public MedicinesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Medicines
		public async Task<IActionResult> Index()

		{
			var claimsIdentity = User.Identity as ClaimsIdentity;
			var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var applicationDbContext = _context.Medicines
        .Include(m => m.Prescription)
        .ThenInclude(p => p.Appointment) // Ensure Appointment is included
        .Where(m => m.Prescription.Appointment.DoctorId == userId || m.Prescription.Appointment.PatientId == userId);
            return View(await applicationDbContext.ToListAsync());
            
        }
		// GET: Medicines/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var medicine = await _context.Medicines
				.Include(m => m.Prescription)
				.FirstOrDefaultAsync(m => m.MedicineId == id);
			if (medicine == null)
			{
				return NotFound();
			}

			return View(medicine);
		}

		// GET: Medicines/Create
		public IActionResult Create()
		{
			ViewData["PrescriptionId"] = new SelectList(_context.Prescriptions, "PrescriptionId", "DiagnosisName");
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("MedicineId,MedicineName,Dose,Duration,Repeat,PrescriptionId")] Medicine medicine)
		{
			if (ModelState.IsValid)
			{
				_context.Add(medicine);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["PrescriptionId"] = new SelectList(_context.Prescriptions, "PrescriptionId", "DiagnosisName", medicine.PrescriptionId);
			return View(medicine);
		}

		// GET: Medicines/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var medicine = await _context.Medicines.FindAsync(id);
			if (medicine == null)
			{
				return NotFound();
			}
			ViewData["PrescriptionId"] = new SelectList(_context.Prescriptions, "PrescriptionId", "DiagnosisName", medicine.PrescriptionId);
			return View(medicine);
		}

		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("MedicineId,MedicineName,Dose,Duration,Repeat,PrescriptionId")] Medicine medicine)
		{
			if (id != medicine.MedicineId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(medicine);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!MedicineExists(medicine.MedicineId))
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
			ViewData["PrescriptionId"] = new SelectList(_context.Prescriptions, "PrescriptionId", "DiagnosisName", medicine.PrescriptionId);
			return View(medicine);
		}

		// GET: Medicines/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var medicine = await _context.Medicines
				.Include(m => m.Prescription)
				.FirstOrDefaultAsync(m => m.MedicineId == id);
			if (medicine == null)
			{
				return NotFound();
			}

			return View(medicine);
		}

		// POST: Medicines/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var medicine = await _context.Medicines.FindAsync(id);
			if (medicine != null)
			{
				_context.Medicines.Remove(medicine);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool MedicineExists(int id)
		{
			return _context.Medicines.Any(e => e.MedicineId == id);
		}
	}
}
