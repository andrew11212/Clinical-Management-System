using Clinical_Management_System.Data;
using Clinical_Management_System.Models;
using Clinical_Management_System.Utitlity;
using Clinical_Management_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Clinical_Management_System.Controllers
{ 
	[Authorize (Policy = Sd.Role_Admin)]
	public class UserManagementController : Controller
	{
		// GET: UserManagementController
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ApplicationDbContext _Context;
		public UserManagementController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_Context = context;
		}

		public async Task<IActionResult> Index()
		{
			var users = _userManager.Users.ToList();
			var userManagementViewModels = new List<UserManagementViewModel>();

			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				userManagementViewModels.Add(new UserManagementViewModel
				{
					UserId = user.Id,
					UserName = user.UserName?? "No UserName",
					Email = user.Email?? "Email is missing",
					Roles = roles
				});
			}

			return View(userManagementViewModels);
		}



		public async Task<IActionResult> Edit(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			var roles = await _roleManager.Roles.ToListAsync();
			var userRoles = await _userManager.GetRolesAsync(user);

			var model = new EditUserRolesViewModel
			{
				UserId = user.Id,
				UserName = user.UserName,
				Roles = roles.Select(r => new SelectListItem
				{
					Value = r.Name,
					Text = r.Name,
					Selected = userRoles.Contains(r.Name)
				}).ToList()
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditUserRolesViewModel model)
		{
			var user = await _userManager.FindByIdAsync(model.UserId);
			if (user == null)
			{
				return NotFound();
			}
			// Ensure SelectedRoles is not null or empty
			if (model.SelectedRoles == null || !model.SelectedRoles.Any())
			{
				ModelState.AddModelError(string.Empty, "Please select at least one role.");
				return View(model); 
			}
			var currentRoles = await _userManager.GetRolesAsync(user);
			await _userManager.RemoveFromRolesAsync(user, currentRoles);
			await _userManager.AddToRolesAsync(user, model.SelectedRoles);

			return RedirectToAction(nameof(Index));
		}


		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: UserManagementController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
