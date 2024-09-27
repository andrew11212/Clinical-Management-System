using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; } [Required]
        public string Type { get; set; } [Required]
        public string Reason { get; set; } [Required]
        public string Notes { get; set; } [Required]
        public TimeSpan Hour { get; set; } [Required]
        public int ClinicId { get; set; }

        public int PatientId { get; set; }
      
        public Clinic Clinic { get; set; }
        public Patient Patient { get; set; }

        public Prescription? Prescription { get; set; }
    }

}
