using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Patient : ApplicationUser
    {
        public ICollection<Document>?Documents { get; set; }
        public ICollection<Allergy> ?Allergies { get; set; }
        public ICollection<ChronicDisease>? ChronicDiseases { get; set; }
        public ICollection<Appointment> ?Appointments { get; set; }
    }

}
