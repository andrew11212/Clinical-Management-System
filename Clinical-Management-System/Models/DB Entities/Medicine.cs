using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string medicine_name { get; set; }
        [Required]
        public string dose { get; set; }
        [Required]
        public string duration { get; set; }
        [Required]
        public int repate { get; set; }
        public int PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public Prescription prescription { get; set; }
    }

}
