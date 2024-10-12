using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Appointment
    {
        [Key]
        [Column("Id")]
        public int AppointementId { get; set; }

        public DateTime Date { get; set; } [Required]

        [MaxLength(100, ErrorMessage ="Appointement Type cannot be greater than 100 charcters")]
        public string? Type { get; set; }
        [MaxLength(200, ErrorMessage = "Reason cannot be greater than 200 charcters")]
        public string? Reason { get; set; }
        [MaxLength(200, ErrorMessage = "Notes cannot be greater than 200 charcters")]
        public string? Notes { get; set; }
        public TimeSpan Hour { get; set; } [Required]
        public int ClinicId { get; set; }

        public string PatientId { get; set; }
        [ValidateNever]
        public Clinic Clinic { get; set; }
        [ValidateNever]
        public Patient Patient { get; set; }
        [ValidateNever]
        public Prescription? Prescription { get; set; }
    }

}
