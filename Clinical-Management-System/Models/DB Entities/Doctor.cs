using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public int SpecializationId { get; set; }
        public byte[]? Photo { get; set; }
        public string NationalId { get; set; }
        public string DoctorUserName { get; set; }
        public string Dpassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public int BuildingNum { get; set; }
        public string StreetName { get; set; }
        public string Government { get; set; }
        public int? Floor { get; set; }

        public Specialization Specialization { get; set; }
    }
}