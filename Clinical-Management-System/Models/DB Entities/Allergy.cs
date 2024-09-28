using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Allergy
    {     
        public string Name { get; set; } =string.Empty;

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
    }
}
