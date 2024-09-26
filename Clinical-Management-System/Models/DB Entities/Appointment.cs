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
        [ForeignKey("ClinicId")]
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Clinic clinic { get; set; }
        public Patient patient { get; set; }
    }

}
