using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Patient
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string NationalId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Government { get; set; }
        [Required]
        public string StreetName { get; set; }

        [Required]
        public int Floor { get; set; }

        [Required]
        public int BuildingNum { get; set; }

        public byte[]? Photo { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Allergy> Allergies { get; set; }
        public ICollection<ChronicDisease> ChronicDiseases { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }

}
