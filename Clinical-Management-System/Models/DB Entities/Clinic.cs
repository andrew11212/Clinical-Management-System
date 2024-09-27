using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Clinic
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public TimeSpan OpenTime { get; set; }
        [Required]

        
        public TimeSpan CloseTime { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public int BuildingNum { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string Government { get; set; }
        [Required]
        public int Floor { get; set; }
      
        public Doctor Doctor { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
