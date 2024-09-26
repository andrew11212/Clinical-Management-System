namespace Clinical_Management_System.Models.DB_Entities
{
    public class Prescription
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string DiagnosisName { get; set; }

        public string? Radiologies { get; set; }
        public string? Laps { get; set; }
        public DateTime? NextVisit { get; set; }

        public Appointment Appointment { get; set; }
    }

}
