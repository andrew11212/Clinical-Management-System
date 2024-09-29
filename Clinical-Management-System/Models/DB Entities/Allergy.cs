using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Allergy
    {
        [Required(ErrorMessage = "Please Provide the name of the Allergy")]
        [MaxLength(100, ErrorMessage = "Allergy name can't be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please Provide the Patient Id")]
        public int PatientId { get; set; }

        public Patient Patient { get; set; } = default!;
    }
}
