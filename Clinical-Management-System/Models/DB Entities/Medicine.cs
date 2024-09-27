using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string MedicineName { get; set; }
        [Required]
        public string Dose { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public int Repeat { get; set; }
  
        public Prescription Prescription { get; set; }
    }

}
