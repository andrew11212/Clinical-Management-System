﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Clinical_Management_System.Data;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Prescription
    {
        
        [Key]
		[Column("Id")]
		public int PrescriptionId { get; set; }
        [Required(ErrorMessage = "Please Provide the Diagnosis Name")]
        [MaxLength(100, ErrorMessage = "Diagnosis name can't be longer than 100 characters")]
        public string DiagnosisName { get; set; }=string.Empty;
        [Required]
        public DateTime DateTime { get; set; }

        
        public int? AppointmentId { get; set; }
        public Appointment? Appointment { get; set; } = default!;


        public ICollection<Medicine>? Medicines { get; set; }

        public ICollection<Document>? Documents { get; set; }
    }

}
