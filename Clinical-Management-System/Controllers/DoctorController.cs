using Clinical_Management_System.Data;
using Clinical_Management_System.Models.DB_Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Clinical_Management_System.Controllers
{
	public class DoctorsController : Controller
	{
		private readonly UserManager<Doctor> _userManager;
		private readonly ApplicationDbContext _context;

		public DoctorsController(UserManager<Doctor> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			this._context = context;
		}

		public IActionResult Index()
		{
			var doctors = _context.Doctors.Include(s=>s.Specialization).ToList();
			return View(doctors);
		}

		
		

		
		
	}
}

