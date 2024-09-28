using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Prescription
    {
        [Key]
		[Column("Id")]
		public int PrescriptionId { get; set; }
        [Required]
        public string DiagnosisName { get; set; } = string.Empty;
        [Required]
        public DateTime DateTime { get; set; }

        
        public int AppointmentId { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public Appointment Appointment { get; set; } = default!;

		public int MedicinesId { get; set; }

		public ICollection<Medicine>? Medicines { get; set; }

        public int DocumentsId { get; set; }
        public ICollection<Document>? Documents { get; set; }
    }

}
