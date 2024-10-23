using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinical_Management_System.ViewModel
{
	public class EditUserRolesViewModel
	{
		public string UserId { get; set; }
		public string UserName { get; set; }
		public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();
		public string[] SelectedRoles { get; set; }
	}
}
