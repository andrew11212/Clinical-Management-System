namespace Clinical_Management_System.Models.DB_Entities
{
    public class Medicine
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public string MedicineName { get; set; }
        public int? Rep { get; set; }
        public string? Dose { get; set; }
        public string? Duration { get; set; }

        public Prescription Prescription { get; set; }
    }

}
