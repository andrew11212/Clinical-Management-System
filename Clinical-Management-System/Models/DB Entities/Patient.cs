using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Patient
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string national_id { get; set; }
        [Required]
        public string user_name { get; set; }
        [Required]
        public string ppassword { get; set; }
        [Required]
        public string first_name { get; set; }
        [Required]
        public string last_name { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string government { get; set; }
        [Required]
        public string street_name { get; set; }

        [Required]
        public int floor { get; set; }

        [Required]
        public int building_num { get; set; }

        public byte[]? Photo { get; set; }
        public List<Document> Documents { get; set; }
        public List<Allergy> Allergy { get; set; }
        public List<ChronicDisease> chronicDiseases { get; set; }
        public List<Appointment> Appointments { get; set; }
    }

}
