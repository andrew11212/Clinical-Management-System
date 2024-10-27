using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinical_Management_System.Models.DB_Entities
{
	public class Appointment
	{
		[Key]
		[Column("Id")]
		public int AppointementId { get; set; } // Primary Key

		[MaxLength(100, ErrorMessage = "Appointment Type cannot be greater than 100 characters")]
		public string? Type { get; set; }

		[MaxLength(200, ErrorMessage = "Reason cannot be greater than 200 characters")]
		public string? Reason { get; set; }

		[MaxLength(200, ErrorMessage = "Notes cannot be greater than 200 characters")]
		public string? Notes { get; set; }

		[MaxLength(200, ErrorMessage = "CreatedBy cannot be greater than 200 characters")]
		public string? CreatedBy { get; set; } = string.Empty;

		public string Status { get; set; } = "Pending";

		[Required]
		public string DoctorId { get; set; } = string.Empty;

		[Required]
		public string PatientId { get; set; } = string.Empty;

		[Required]
		public int ScheduleId { get; set; }

		[Required]
		public int ClinicId { get; set; }

		// Navigation properties
		[ValidateNever]
		[ForeignKey(nameof(DoctorId))]
		public Doctor Doctor { get; set; } = default!;

		[ValidateNever]
		[ForeignKey(nameof(PatientId))]
		public Patient Patient { get; set; } = default!;

		[ValidateNever]
		[ForeignKey(nameof(ScheduleId))]
		public Schedule Schedule { get; set; } = default!;

		[ValidateNever]
		[ForeignKey(nameof(ClinicId))]
		public Clinic Clinic { get; set; } = default!;

		[ValidateNever]
		public ICollection< Prescription>? Prescriptions { get; set; }
	}
}
