using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Clinical_Management_System.Models.DB_Entities
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public byte[] Image { get; set; }

       
        public Patient Patient { get; set; }
        public Prescription? Prescription { get; set; }
    }

}
