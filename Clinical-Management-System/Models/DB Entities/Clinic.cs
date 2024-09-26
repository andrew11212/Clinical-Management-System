namespace Clinical_Management_System.Models.DB_Entities
{
    public class Clinic
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public TimeSpan OpenClose { get; set; }
        public string City { get; set; }
        public int BuildingNum { get; set; }
        public string StreetName { get; set; }
        public string Government { get; set; }
        public int? Floor { get; set; }

        public Doctor Doctor { get; set; }
    }
}
