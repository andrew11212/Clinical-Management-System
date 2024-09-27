using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Specialization
    {
        [Key]
        public int SpecializationId { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Specialization Name cannot be more than 100 characters")]

        public string SpecializationName { get; set; }


        public ICollection<Doctor>? Doctors { get; set; }
    }
}
