using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Clinic
    {
        [Key]
        public int ClinicId { get; set; }


        [Required]
        public TimeSpan OpenTime { get; set; }
        [Required]
        public TimeSpan CloseTime { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "City name can't be longer than 100 characters")]
        public string City { get; set; }
        [Required]
        public int BuildingNum { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Street name can't be longer than 100 characters")]
        public string StreetName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Government name can't be longer than 100 characters")]
        public string Government { get; set; }
        [Required]
        public int Floor { get; set; }

        public int DoctorId { get; set; }
      
        public Doctor Doctor { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
