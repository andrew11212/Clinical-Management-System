using Clinical_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinical_Management_System.ViewModel
{
	public class UserManagementViewModel
	{
		public string UserId { get; set; } =string.Empty;
		public string UserName { get; set; }=string.Empty;
		public string Email { get; set; }=string.Empty;
		public IList<string> Roles { get; set; } = default!;
	}

}
