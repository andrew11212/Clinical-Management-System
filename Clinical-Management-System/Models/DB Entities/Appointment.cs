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

        public string Type { get; set; } = string.Empty; [Required] 
        public string Reason { get; set; } = string.Empty; [Required]
        public string Notes { get; set; } = string.Empty; [Required]
        public TimeSpan Hour { get; set; } [Required]
        public int ClinicId { get; set; }

        public Clinic Clinic { get; set; } = default!;
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = default!;

        public Prescription? Prescription { get; set; }
    }

}
