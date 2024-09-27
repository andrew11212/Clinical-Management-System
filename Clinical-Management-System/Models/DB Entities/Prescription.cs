using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }
        [Required]
        public string DiagnosisName { get; set; }
        [Required]
        public DateTime DateTime { get; set; }

        
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }


        public ICollection<Medicine> Medicines { get; set; }

        public ICollection<Document>? Documents { get; set; }
    }

}
