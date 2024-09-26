namespace Clinical_Management_System.Models.DB_Entities
{
    public class Document
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public byte[]? PrevioseResults { get; set; }
        public byte[]? UploadedResult { get; set; }

        public Patient Patient { get; set; }
    }

}
