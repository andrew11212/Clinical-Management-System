using Microsoft.EntityFrameworkCore;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Allergy
    {     
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string  ? AllergyName { get; set; }

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}
