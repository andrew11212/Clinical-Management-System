using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Medicine
    {
        [Key]
		[Column("Id")]
		public int MedicineId { get; set; }
        [Required]
        public string MedicineName { get; set; }=string.Empty;
        [Required]
        public string Dose { get; set; } =string.Empty;
        [Required]
        public string Duration { get; set; } = string.Empty;
		[Required]
        public int Repeat { get; set; }
        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; } = default!;
	}

}
