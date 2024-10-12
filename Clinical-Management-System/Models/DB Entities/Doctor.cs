using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Identity;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Doctor: IdentityUser
    { 
    
        public byte[]? Photo { get; set; }
        [Required]
        [StringLength(14, ErrorMessage = "National Id must be 14 characters", MinimumLength = 14)]
        public string NationalId { get; set; } = string.Empty;
		[Required]
        [MaxLength(100, ErrorMessage = "Username cannot be greater than 100 charcters")]
        public string UserName { get; set; } = string.Empty;
		[Required]
        [MaxLength(100, ErrorMessage = "Password cannot be greater than 100 charcters")]
        public string Password { get; set; } = string.Empty;
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
        public int  Floor { get; set; }
        //public List<Clinic> Clinics { get; set; }
        public int SpecializationId { get; set; }

        [ValidateNever]
        public Specialization Specialization { get; set; } = default!;

     
        public ICollection<Clinic>? Clinics { get; set; }
    }
}