namespace Clinical_Management_System.Models.DB_Entities
{
    public class ChronicDisease
    {
        public string Name { get; set; }
        public int PatientId { get; set; }

        public Patient Patient { get; set; }
    }

}
