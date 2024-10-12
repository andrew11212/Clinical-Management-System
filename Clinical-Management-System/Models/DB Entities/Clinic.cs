using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Clinic
    {
        [Key]
		[Column("Id")]
		public int ClinicId { get; set; }


        [Required]
        public TimeSpan OpenTime { get; set; }
        [Required]
        public TimeSpan CloseTime { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "City name can't be longer than 100 characters")]
        public string City { get; set; } = string.Empty;
		[Required]
        public int BuildingNum { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Street name can't be longer than 100 characters")]
        public string StreetName { get; set; } = string.Empty;
		[Required]
        [MaxLength(100, ErrorMessage = "Government name can't be longer than 100 characters")]
        public string Government { get; set; } = string.Empty;
		[Required]
        public int Floor { get; set; }

        public string DoctorId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; } = default!;

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
