using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public byte[] Dimage { get; set; }

        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient patient { get; set; }
        public int PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public Prescription prescription { get; set; }
    }

}
