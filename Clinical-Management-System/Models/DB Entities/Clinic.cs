using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Clinic
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public TimeSpan open_time { get; set; }
        [Required]

        
        public TimeSpan close_time { get; set; }
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
      

        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor doctor { get; set; }
    }
}
