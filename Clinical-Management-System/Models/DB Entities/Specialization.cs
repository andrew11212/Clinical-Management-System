using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Specialization
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public string Specialization_Name { get; set; }
    }
}
