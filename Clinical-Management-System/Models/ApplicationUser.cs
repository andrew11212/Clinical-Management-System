using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [Display(Name = "Phone Number")]
        public byte[]? Photo { get; set; }
        [Required]
        [StringLength(14, ErrorMessage = "National Id must be 14 characters", MinimumLength = 14)]
        public string NationalId { get; set; } = string.Empty;
        [Required]
        [MaxLength(100, ErrorMessage = "Username cannot be greater than 100 charcters")]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100, ErrorMessage = "First name cannot be greater than 100 charcters")]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100, ErrorMessage = "Last name cannot be greater than 100 charcters")]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100, ErrorMessage = "Email cannot be greater than 100 charcters")]
        public string City { get; set; } = string.Empty;
        [Required]
        public int BuildingNum { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Street name cannot be greater than 100 charcters")]
        public string StreetName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100, ErrorMessage = "Government cannot be greater than 100 charcters")]
        public string Government { get; set; } = string.Empty;
        [Required]
        public int Floor { get; set; }
        public int SpecializationId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> SpecializationList { get; set; } = default!;
    }
}
