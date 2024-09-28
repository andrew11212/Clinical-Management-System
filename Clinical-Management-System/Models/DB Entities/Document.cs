using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Document
    {
        [Key]
		[Column("Id")]
		public int DocumentId { get; set; }

        public DateTime CreatedDate { get; set; }
        public byte[] Image { get; set; } = default!;

        public int PatientId { get; set; }
        public Patient Patient { get; set; }=default!;
        public int? PrescriptionId { get; set; }
        public Prescription? Prescription { get; set; }
        
    }

}
