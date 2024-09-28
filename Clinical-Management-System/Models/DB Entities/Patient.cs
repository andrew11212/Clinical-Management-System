using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Patient
    {

        [Key]
		[Column("Id")]
		public int PatientId { get; set; }
        [Required]
        public string NationalId { get; set; } =string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
		[Required]
        public string Password { get; set; } = string.Empty;
		[Required]
        public string FirstName { get; set; } = string.Empty;
		[Required]
        public string LastName { get; set; } = string.Empty;
		[Required]
        public string City { get; set; } = string.Empty;
		[Required]
        public string Government { get; set; } = string.Empty;
		[Required]
        public string StreetName { get; set; } = string.Empty;

		[Required]
        public int Floor { get; set; }

        [Required]
        public int BuildingNum { get; set; }

        public byte[]? Photo { get; set; }
        public ICollection<Document>? Documents { get; set; } 
        public ICollection<Allergy>? Allergies { get; set; }
        public ICollection<ChronicDisease>? ChronicDiseases { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }

}
