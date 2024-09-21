using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models
{
	public class ApplicationUser:IdentityUser
	{

		[Required(ErrorMessage = "Full name is required")]
		[StringLength(100, ErrorMessage = "Full name can't be longer than 100 characters")]
		public string FullName { get; set; } = string.Empty;


		[MaxLength(200, ErrorMessage = "Address can't be longer than 200 characters")]
		public string? Address { get; set; } = string.Empty;

		[MaxLength(200, ErrorMessage = "Chronic disease description can't be longer than 200 characters")]
		public string? ChronicDisease { get; set; } = string.Empty;

		[MaxLength(500, ErrorMessage = "Photo URL can't be longer than 500 characters")]
		public string? Photo { get; set; } = string.Empty;

		[MaxLength(200, ErrorMessage = "Allergy description can't be longer than 200 characters")]
		public string?Allergy { get; set; } = string.Empty;

		[MaxLength(100, ErrorMessage = "Clinic name can't be longer than 100 characters")]
		public string? Clinic { get; set; } = string.Empty;

		[MaxLength(100, ErrorMessage = "Specialization can't be longer than 100 characters")]
		public string? Specialization { get; set; } = string.Empty;

		[MaxLength(100, ErrorMessage = "Location can't be longer than 100 characters")]
		public string? Location { get; set; } = string.Empty;

	}
}
