using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Diagnosis_Name { get; set; }
        [Required]
        public DateTime dateTime { get; set; }

        public List<Medicine> Medicines { get; set; }
        public int AppointmentId { get; set; }
        [ForeignKey("AppointmentId")]
        public Appointment appointment { get; set; }
    }

}
