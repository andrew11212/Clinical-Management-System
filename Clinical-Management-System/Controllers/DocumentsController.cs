using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinical_Management_System.Data;
using Clinical_Management_System.Models.DB_Entities;
using System.IO;
using System;
using Clinical_Management_System.Utitlity;

namespace Clinical_Management_System.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DocumentsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var documentList = await _context.Documents
                .Where(d => d.Prescription.Appointment.PatientId == userId || d.Prescription.Appointment.DoctorId == userId)
                .Include(d => d.Patient)
                .Include(d => d.Prescription)
                .ToListAsync();
            return View(documentList);
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Patient)
                .Include(d => d.Prescription)
                .FirstOrDefaultAsync(m => m.DocumentId == id);

            return document == null ? NotFound() : View(document);
        }

        // GET: Documents/Create
        [Authorize(Policy = Sd.Role_Patient)]
        public IActionResult Create()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["PrescriptionId"] = new SelectList(_context.Prescriptions.Where(c => c.Appointment.PatientId == userId), "PrescriptionId", "DiagnosisName");
            return View();
        }

        // POST: Documents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Sd.Role_Patient)]
        public async Task<IActionResult> Create(Document document, IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User ID not found");
            }
            document.PatientId = userId;

            if (!ModelState.IsValid)
            {
                return await HandleInvalidModel(document);
            }

            try
            {
                // Save the file and update the document image path
                await SaveDocumentAsync(document, file);
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while saving the document.");
            }

            return await HandleInvalidModel(document);
        }

        // GET: Documents/Edit/5
        [HttpGet]
        [Authorize(Policy = Sd.Role_Patient)]
        public async Task<IActionResult> Edit(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null) return NotFound();

            ViewData["PrescriptionId"] = new SelectList(_context.Prescriptions, "PrescriptionId", "DiagnosisName", document.PrescriptionId);
            return View(document);
        }

        // POST: Documents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Sd.Role_Patient)]
        public async Task<IActionResult> Edit(int id, Document document, IFormFile? file)
        {
            if (id != document.DocumentId) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || document.PatientId != userId)
            {
                return Unauthorized("You are not authorized to edit this document.");
            }

            if (!ModelState.IsValid)
            {
                return await HandleInvalidModel(document);
            }

            try
            {
                var existingDocument = await _context.Documents.FindAsync(id);
                if (existingDocument == null) return NotFound();

                // Update fields
                existingDocument.CreatedDate = document.CreatedDate;
                existingDocument.PrescriptionId = document.PrescriptionId;

                // Handle file upload
                await UpdateDocumentImageAsync(existingDocument, file);
                _context.Update(existingDocument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while editing the document.");
            }

            return await HandleInvalidModel(document);
        }

        // GET: Documents/Delete/5
        [Authorize(Policy = Sd.Role_Patient)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var document = await _context.Documents
                .Include(d => d.Patient)
                .Include(d => d.Prescription)
                .FirstOrDefaultAsync(m => m.DocumentId == id && m.PatientId == userId);

            return document == null ? NotFound() : View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Sd.Role_Patient)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var document = await _context.Documents.FirstOrDefaultAsync(m => m.DocumentId == id && m.PatientId == userId);
            if (document != null)
            {
                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> HandleInvalidModel(Document document)
        {
            ViewData["PrescriptionId"] = new SelectList(_context.Prescriptions, "PrescriptionId", "DiagnosisName", document.PrescriptionId);
            return View(document);
        }

        private async Task SaveDocumentAsync(Document document, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string documentPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "document");
                if (!Directory.Exists(documentPath))
                {
                    Directory.CreateDirectory(documentPath);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string fullPath = Path.Combine(documentPath, fileName);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                document.Image = Path.Combine("images", "document", fileName).Replace("\\", "/");
            }
        }

        private async Task UpdateDocumentImageAsync(Document existingDocument, IFormFile? file)
        {
            if (file != null && file.Length > 0)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(existingDocument.Image))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingDocument.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save the new file
                await SaveDocumentAsync(existingDocument, file);
            }
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.DocumentId == id);
        }
    }
}
