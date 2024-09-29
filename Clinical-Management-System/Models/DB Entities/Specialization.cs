using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Specialization
    {
        [Key]
		[Column("Id")]
		public int SpecializationId { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Specialization Name cannot be more than 100 characters")]

        public string SpecializationName { get; set; }=string.Empty;


        public ICollection<Doctor>? Doctors { get; set; }
    }
}
