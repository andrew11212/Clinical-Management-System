using Clinical_Management_System.Models;
using Clinical_Management_System.Models.DB_Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Clinical_Management_System.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<ApplicationUser> applicationUsers { get; set; }

        #region DB Entities
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<ChronicDisease> ChronicDiseases { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
		{
            builder.Entity<Allergy>().HasKey(a => new { a.PatientId, a.DoctorId });

            builder.Entity<ChronicDisease>().HasKey(cd => new { cd.PatientId, cd.DoctorId });

            base.OnModelCreating(builder);
		}
	}
}
