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
        [Authorize(Policy = Sd.Role_Patient)]
        public IActionResult Create()
        {
            var claims = User.Identity as ClaimsIdentity;
            var userId = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["PrescriptionId"] = new SelectList(_context.Prescriptions.Where(c => c.Appointment.PatientId == userId), "PrescriptionId", "DiagnosisName");
            return View();
        }

        // POST: Documents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Sd.Role_Patient)]
        public async Task<IActionResult> Create(Document document, IFormFile file)
        {
            var claims = User.Identity as ClaimsIdentity;
            var userId = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return NotFound("User ID not found");
            }
            document.PatientId = userId;

            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    Console.WriteLine($"wwwRootPath: {wwwRootPath}"); // Debugging line
                    string documentPath = Path.Combine(wwwRootPath, "images", "document");

                    // Ensure the directory exists
                    if (!Directory.Exists(documentPath))
                    {
                        Directory.CreateDirectory(documentPath);
                        Console.WriteLine($"Directory created: {documentPath}"); // Debugging line
                    }

                    if (file != null && file.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string fullPath = Path.Combine(documentPath, fileName);
                        Console.WriteLine($"Full file path: {fullPath}"); // Debugging line

                        // Saving the file
                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            Console.WriteLine("File saved successfully."); // Debugging line
                        }

                        // Update the document's image path
                        document.Image = Path.Combine("images", "document", fileName).Replace("\\", "/");
                        Console.WriteLine($"Image path to be saved in DB: {document.Image}"); // Debugging line
                    }
                    else
                    {
                        Console.WriteLine("File is null or has zero length."); // Debugging line
                    }

                    // Add to context and save
                    _context.Add(document);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Document saved in the database."); // Debugging line
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}"); // Log the error
                    return View(document);
                }
            }
            else
            {
                Console.WriteLine("ModelState is not valid."); // Debugging line
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}"); // Log all validation errors
                }
            }

            // If we got this far, something failed; re-display form
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FirstName", document.PatientId);
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

            var document = await _context.Documents.Where(c => c.PatientId == userId).FirstOrDefaultAsync(c => c.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }
            ViewData["PrescriptionId"] = new SelectList(_context.Prescriptions, "PrescriptionId", "DiagnosisName", document.PrescriptionId);
            return View(document);
        }

        // POST: Documents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Sd.Role_Patient)]
        public async Task<IActionResult> Edit(int id, Document document, IFormFile file)
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
                    if (file != null && file.Length > 0)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string documentPath = Path.Combine(wwwRootPath, "images", "document");

                        // Ensure the directory exists
                        if (!Directory.Exists(documentPath))
                        {
                            Directory.CreateDirectory(documentPath);
                        }

                        // Delete old image if it exists
                        if (!string.IsNullOrEmpty(document.Image))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, document.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        using (var fileStream = new FileStream(Path.Combine(documentPath, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        document.Image = Path.Combine("images", "document", fileName).Replace("\\", "/");
                    }

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
        [Authorize(Policy = Sd.Role_Patient)]
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
