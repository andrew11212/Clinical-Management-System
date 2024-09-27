using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        
    
        public byte[]? Photo { get; set; }
        [Required]
        [StringLength(14, ErrorMessage = "National Id must be 14 characters", MinimumLength = 14)]
        public string NationalId { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Username cannot be greater than 100 charcters")]
        public string UserName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Password cannot be greater than 100 charcters")]
        public string Password { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "First name cannot be greater than 100 charcters")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Last name cannot be greater than 100 charcters")]
        public string LastName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Email cannot be greater than 100 charcters")]
        public string City { get; set; }
        [Required]
        public int BuildingNum { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Street name cannot be greater than 100 charcters")]
        public string StreetName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Government cannot be greater than 100 charcters")]
        public string Government { get; set; }
        [Required]
        public int  Floor { get; set; }
        //public List<Clinic> Clinics { get; set; }
        public int SpecializationId { get; set; }
 
        public Specialization Specialization { get; set; }

     
        public ICollection<Clinic>? Clinics { get; set; }
    }
}