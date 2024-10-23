using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Identity;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Doctor: ApplicationUser
    { 
    
        public int SpecializationId { get; set; }

        [ValidateNever]
        public Specialization Specialization { get; set; } = default!;

        public ICollection<Appointment>? Appointments { get; set; }
		public ICollection<Clinic>? Clinics { get; set; }
    }
}