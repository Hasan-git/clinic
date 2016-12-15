using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Clinic.Core.Domain.Models;

namespace Clinic.Infrastructure.Data
{
    public class ClinicContext : DbContext
    {
        public ClinicContext()
            : base("Clinic")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<Appointment>().ToTable("Appointment");
            modelBuilder.Entity<Consultation>().ToTable("Consultation");
            modelBuilder.Entity<FollowUp>().ToTable("FollowUp");
            modelBuilder.Entity<Doctor>().ToTable("Doctor");
            modelBuilder.Entity<Assistant>().ToTable("Assistant");
            modelBuilder.Entity<Core.Domain.Models.Clinic>().ToTable("Clinic");
            modelBuilder.Entity<Images>().ToTable("Images");

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Consultation> Consultations { get; set; }
        public virtual DbSet<FollowUp> FollowUps { get; set; }
        public virtual  DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Assistant> Assistants { get; set; }
        public virtual DbSet<Core.Domain.Models.Clinic> Clinics { get; set; }
        public virtual DbSet<Images> Images { get; set; }

    }
}

