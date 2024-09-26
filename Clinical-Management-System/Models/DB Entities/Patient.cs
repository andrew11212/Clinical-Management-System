namespace Clinical_Management_System.Models.DB_Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string Ppassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public byte[]? Photo { get; set; }
        public string City { get; set; }
        public int BuildingNum { get; set; }
        public string StreetName { get; set; }
        public string Government { get; set; }
        public int? Floor { get; set; }
    }

}
