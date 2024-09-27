using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Medicine
    {
        [Key]
        public int MedicineId { get; set; }
        [Required(ErrorMessage = "Please Provide the Medicine Name")]
        [MaxLength(100, ErrorMessage = "Medicine name can't be longer than 100 characters")]
        public string MedicineName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Dose can't be longer than 100 characters")]
        public string Dose { get; set; }
        [Required]
        [MaxLength(200, ErrorMessage = "Duration can't be longer than 200 characters")]
        public string Duration { get; set; }
        [Required]
        public int Repeat { get; set; }
        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }
    }

}
