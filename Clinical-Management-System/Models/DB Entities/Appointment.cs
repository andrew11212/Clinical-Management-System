namespace Clinical_Management_System.Models.DB_Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public int ClinicId { get; set; }
        public int PatientId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public TimeSpan Hour { get; set; }

        public Clinic Clinic { get; set; }
        public Patient Patient { get; set; }
    }

}
