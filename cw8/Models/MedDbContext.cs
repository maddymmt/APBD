using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw8.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace cw8.Models
{
    public class MedDbContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Prescription_Medicament> PrescriptionMedicaments { get; set; }
        
        

        public MedDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    IdDoctor = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jankowalski@gmail.com"
                },
                new Doctor
                {
                    IdDoctor = 2, FirstName = "Anna", LastName = "Nowak", Email = "annanowak@gmail.com"
                }
            };

            var patients = new List<Patient>
            {
                new Patient
                {
                    IdPatient = 1, FirstName = "Antoni", LastName = "Duda", Birthdate = DateTime.Parse("2000-01-01")
                },
                new Patient
                {
                    IdPatient = 2, FirstName = "Jakub", LastName = "Solis", Birthdate = DateTime.Parse("2001-01-01")
                }
            };

            var prescriptions = new List<Prescription>
            {
                new Prescription
                {
                    IdPrescription = 1, Date = DateTime.Parse("2020-01-01"), DueDate = DateTime.Parse("2020-02-01"),
                    IdPatient = 1, IdDoctor = 1
                },
                new Prescription
                {
                    IdPrescription = 2, Date = DateTime.Parse("2021-01-01"), DueDate = DateTime.Parse("2021-03-01"),
                    IdPatient = 2, IdDoctor = 2
                }
            };
            var medicaments = new List<Medicament>
            {
                new Medicament
                {
                    IdMedicament = 1, Name = "Paracetamol", Description = "Lek przeciwbólowy", Type = "Lek bólowy"
                },
                new Medicament
                {
                    IdMedicament = 2, Name = "Ibuprofen", Description = "Lek przeciwgorączkowaniu", Type = "Lek gorączkowy"
                }
            };
            var prescriptionMedicaments = new List<Prescription_Medicament>
            {
                new Prescription_Medicament
                {
                    IdMedicament = 1, IdPrescription = 1, Dose = 10, Details = "Dwa dni po zjedzeniu posiłku"
                },
                new Prescription_Medicament
                {
                    IdMedicament = 2, IdPrescription = 2, Dose = 5, Details = "Dwa dni po zjedzeniu posiłku"
                }
            };
            


            modelBuilder.Entity<Doctor>(e =>
            {
                e.HasKey(k => k.IdDoctor);
                e.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
                e.Property(p => p.LastName).IsRequired().HasMaxLength(100);
                e.Property(p => p.Email).IsRequired().HasMaxLength(100);
                e.HasData(doctors);
                e.ToTable("Doctor");
            });
            modelBuilder.Entity<Patient>(e =>
            {
                e.HasKey(k => k.IdPatient);
                e.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
                e.Property(p => p.LastName).IsRequired().HasMaxLength(100);
                e.Property(p => p.Birthdate).IsRequired();
                e.HasData(patients);
                e.ToTable("Patient");
            });
            modelBuilder.Entity<Prescription>(e =>
            {
                e.HasKey(k => k.IdPrescription);
                e.Property(p => p.Date).IsRequired();
                e.Property(p => p.DueDate).IsRequired();
                e.HasOne(e => e.Doctor)
                    .WithMany(e => e.Prescriptions)
                    .HasForeignKey(e => e.IdPatient)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(e => e.Patient)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(p => p.IdDoctor)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasData(prescriptions);
                e.ToTable("Prescription");
            });
            modelBuilder.Entity<Medicament>(e =>
            {
                e.HasKey(k => k.IdMedicament);
                e.Property(p => p.Name).IsRequired().HasMaxLength(100);
                e.Property(p => p.Description).IsRequired().HasMaxLength(100);
                e.Property(p => p.Type).IsRequired().HasMaxLength(100);
                e.HasData(medicaments);
                e.ToTable("Medicament");
            });
            modelBuilder.Entity<Prescription_Medicament>(e =>
            {
                e.HasKey(k => new { k.IdMedicament, k.IdPrescription });
                e.HasOne(e => e.Medicament)
                    .WithMany(e => e.PrescriptionMedicaments)
                    .HasForeignKey(e => e.IdMedicament)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(e => e.Prescription)
                    .WithMany(e => e.PrescriptionMedicaments)
                    .HasForeignKey(e => e.IdPrescription)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasData(prescriptionMedicaments);
                e.ToTable("Prescription_Medicament");
            });
        }
    }
}