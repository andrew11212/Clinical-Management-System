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
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var documentList = _context.Documents
				.Where(d => d.Prescription.Appointment.PatientId == userId || d.Prescription.Appointment.DoctorId == userId)
				.Include(d => d.Patient) 
				.Include(d => d.Prescription) 
				.ToListAsync();
            return View(await documentList);
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Patient)
                .Include(d => d.Prescription)
                .FirstOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        [Authorize(Policy =Sd.Role_Patient)]
        public IActionResult Create()
        {
			var claims = User.Identity as ClaimsIdentity;
			var userId = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			ViewData["PrescriptionId"] = new SelectList(_context.Prescriptions.Where(c=>c.Appointment.PatientId==userId), "PrescriptionId", "DiagnosisName");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Document document)
        {
			var claims = User.Identity as ClaimsIdentity;
			var userId = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return NotFound();
            }
            document.PatientId = userId;
			if (ModelState.IsValid)
            {
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FirsName", document.PatientId);
            ViewData["PrescriptionId"] = new SelectList(_context.Prescriptions, "PrescriptionId", "DiagnosisName", document.PrescriptionId);
            return View(document);
        }

		// GET: Documents/Edit/5
		[Authorize(Policy = Sd.Role_Patient)]

		public async Task<IActionResult> Edit(int? id)
        {
			var claims = User.Identity as ClaimsIdentity;
			var userId = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.Where(c=>c.PatientId==userId).FirstOrDefaultAsync(c=>c.DocumentId==id);
            if (document == null)
            {
                return NotFound();
            }
            ViewData["PrescriptionId"] = new SelectList(_context.Prescriptions, "PrescriptionId", "DiagnosisName", document.PrescriptionId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Document document)
        {
			var claims = User.Identity as ClaimsIdentity;
			var userId = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (id != document.DocumentId)
            {
                return NotFound();
            }
            document.PatientId = userId;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.DocumentId))
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
            ViewData["PrescriptionId"] = new SelectList(_context.Prescriptions, "PrescriptionId", "DiagnosisName", document.PrescriptionId);
            return View(document);
        }

		// GET: Documents/Delete/5
		[Authorize(Policy = Sd.Role_Patient)]
		public async Task<IActionResult> Delete(int? id)
        {
			var claims = User.Identity as ClaimsIdentity;
			var userId = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Patient)
                .Include(d => d.Prescription)
				.Where(c => c.PatientId == userId).FirstOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			var claims = User.Identity as ClaimsIdentity;
			var userId = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var document = await _context.Documents.Where(c => c.PatientId == userId).FirstOrDefaultAsync(m => m.DocumentId == id);
			if (document != null)
            {
                _context.Documents.Remove(document);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.DocumentId == id);
        }
    }
}
