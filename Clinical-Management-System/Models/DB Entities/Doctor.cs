using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        
    
        public byte[]? Photo { get; set; }
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
        public int BuildingNum { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string Government { get; set; }
        [Required]
        public int  Floor { get; set; }
        //public List<Clinic> Clinics { get; set; }
 
        public Specialization Specialization { get; set; }

        public ICollection<Clinic>? Clinics { get; set; }
    }
}