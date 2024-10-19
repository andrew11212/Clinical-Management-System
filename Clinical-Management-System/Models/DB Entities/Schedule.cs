using Clinical_Management_System.Models.DB_Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Schedule
{
	[Key]
	public int Id { get; set; }

	[Required]
	public string DoctorId { get; set; } = string.Empty; // Foreign Key to Doctor

	[Required]
	public DateTime AvailableDateTime { get; set; } // The available date and time for appointments

	[Required]
	[MaxLength(50)]
	public string Status { get; set; } = "Available"; // Status of the time slot (e.g., Available, Booked, Cancelled)

	[ValidateNever]
	[ForeignKey(nameof(DoctorId))]
	public Doctor Doctor { get; set; } = default!; // Navigation property to Doctor

	public ICollection<Appointment>? Appointments { get; set; } // Optional, to track any appointments linked to this schedule
}
