﻿using Clinical_Management_System.Models;
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
		public DbSet<ApplicationUser> applicationUsers { get; set; }



		public DbSet<Schedule> Schedule { get; set; }
		public bool GetBendingMigrations { get; internal set; }
		#endregion

		protected override void OnModelCreating(ModelBuilder builder)
		{
			// Mapping Prescriptions to Documents with Restrict delete behavior
			builder.Entity<Prescription>()
				.HasMany(p => p.Documents)
				.WithOne(d => d.Prescription)
				.HasForeignKey(d => d.PrescriptionId)
				.OnDelete(DeleteBehavior.Restrict);

			// Composite keys for Allergy and ChronicDisease
			builder.Entity<Allergy>().HasKey(a => new { a.PatientId, a.Name });
			builder.Entity<ChronicDisease>().HasKey(cd => new { cd.PatientId, cd.Name });

			// Mapping Prescriptions to Medicines with Cascade delete behavior
			builder.Entity<Prescription>()
				.HasMany(p => p.Medicines)
				.WithOne(m => m.Prescription)
				.HasForeignKey(m => m.PrescriptionId)
				.OnDelete(DeleteBehavior.Cascade);

			// Mapping Patients to Documents with Restrict delete behavior
			builder.Entity<Patient>()
				.HasMany(p => p.Documents)
				.WithOne(d => d.Patient)
				.OnDelete(DeleteBehavior.Restrict);

			// Mapping Patients to other entities with Cascade delete behavior
			builder.Entity<Patient>()
				.HasMany(p => p.Allergies)
				.WithOne(a => a.Patient)
				.OnDelete(DeleteBehavior.Cascade);
			builder.Entity<Patient>()
				.HasMany(p => p.ChronicDiseases)
				.WithOne(c => c.Patient)
				.OnDelete(DeleteBehavior.Cascade);
			builder.Entity<Patient>()
				.HasMany(p => p.Appointments)
				.WithOne(a => a.Patient)
				.OnDelete(DeleteBehavior.Cascade);

			// Doctor to Clinic relationship with Restrict to avoid cascade conflict
			builder.Entity<Doctor>()
				.HasMany(d => d.Clinics)
				.WithOne(c => c.Doctor)
				.OnDelete(DeleteBehavior.Restrict); // Prevent multiple cascade paths

			// Specialization to Doctor relationship with Cascade
			builder.Entity<Specialization>()
				.HasMany(s => s.Doctors)
				.WithOne(d => d.Specialization)
				.OnDelete(DeleteBehavior.Cascade);
			builder.Entity<Prescription>()
				.HasOne(p => p.Appointment)
				.WithMany(a => a.Prescriptions)
				.HasForeignKey(p => p.AppointmentId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Document>()
	   .HasOne(d => d.Prescription)
	   .WithMany(p => p.Documents)
	   .HasForeignKey(d => d.PrescriptionId)
	   .OnDelete(DeleteBehavior.Cascade);

			base.OnModelCreating(builder);




		}


	}
}
