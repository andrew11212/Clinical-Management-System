using Microsoft.EntityFrameworkCore;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Allergy
    {     
        public string Name { get; set; }

        public Patient Patient { get; set; }
    }
}
