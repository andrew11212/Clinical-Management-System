using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Doctor
    {
        [Key]
		[Column("Id")]
		public int DoctorId { get; set; }
    
        public byte[]? Photo { get; set; }
        [Required]
        public string NationalId { get; set; }=string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; }=string.Empty;
        [Required]
        public string FirstName { get; set; }=string.Empty ;
        [Required]
        public string LastName { get; set; } =string.Empty;
        [Required]
        public string City { get; set; }=string.Empty;
        [Required]
        public int BuildingNum { get; set; }
        [Required]
        public string StreetName { get; set; } = string.Empty;
        [Required]
        public string Government { get; set; } = string.Empty;
		[Required]
        public int  Floor { get; set; }
        //public List<Clinic> Clinics { get; set; }
        public int SpecializationId { get; set; }

        public Specialization Specialization { get; set; } = default!;

        public int ClinicsId { get; set; }
        public ICollection<Clinic>? Clinics { get; set; }
    }
}