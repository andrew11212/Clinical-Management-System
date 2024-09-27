namespace Clinical_Management_System.Models.DB_Entities
{
    public class ChronicDisease
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string ? DiseaseName { get; set; }

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }

}
