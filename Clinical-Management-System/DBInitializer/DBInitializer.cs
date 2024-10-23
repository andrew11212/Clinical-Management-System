using Clinical_Management_System.Data;
using Clinical_Management_System.DbInitializer;
using Clinical_Management_System.Models;
using Clinical_Management_System.Utitlity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinical_Management_System.DBInitializer
{
	public class DBInitializer : IDBInitializer
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ApplicationDbContext _context;

		public DBInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
		}

		public void Initialize()
		{
			try
			{
				if (_context.Database.GetPendingMigrations().Count() > 0)
				{
					_context.Database.Migrate();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error applying migrations: {ex.Message}");
				return;
			}


			if (!_roleManager.RoleExistsAsync(Sd.Role_Patient).GetAwaiter().GetResult())
			{

				{
					_roleManager.CreateAsync(new IdentityRole(Sd.Role_Patient)).GetAwaiter().GetResult();
					_roleManager.CreateAsync(new IdentityRole(Sd.Role_Admin)).GetAwaiter().GetResult();
					_roleManager.CreateAsync(new IdentityRole(Sd.Role_Doctor)).GetAwaiter().GetResult();



					// Create admin user if roles are successfully created
					var result = _userManager.CreateAsync(new ApplicationUser
					{
						UserName = "AdminAndrew@Email.com",
						Email = "AdminAndrew@Email.com",
						FirstName = "Andrew",
						LastName = "Atef",
						PhoneNumber = "1112334455",
						StreetName = "aasssdd",
						Government = "Egypt",
						City = "Cairo",
					}, password: "Andrew@11").GetAwaiter().GetResult();
					if (result.Succeeded)
					{
						_userManager.AddToRoleAsync(_userManager.FindByEmailAsync("AdminAndrew@Email.com").GetAwaiter().GetResult(), Sd.Role_Admin).GetAwaiter().GetResult();
					}

				}
			}
		}
	}
}