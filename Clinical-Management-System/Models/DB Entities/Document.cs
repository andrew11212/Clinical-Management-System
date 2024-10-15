using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Document
    {
        [Key]
		[Column("Id")]
		public int DocumentId { get; set; }

        public DateTime CreatedDate { get; set; }

        [ValidateNever] 
        public string? Image { get; set; } = string.Empty;

        public string PatientId { get; set; }=string.Empty;
        public int? PrescriptionId { get; set; }
		[ValidateNever]
		public Patient Patient { get; set; } = default!;
		[ValidateNever]
		public Prescription? Prescription { get; set; }
    }

}
