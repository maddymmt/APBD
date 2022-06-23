using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw8.Models;
using cw8.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace cw8.Services
{
    public class MedService : IMedservice
    {
        private readonly MedDbContext _context;

        public MedService(MedDbContext context)
        {
            _context = context;
        }

        public async Task AddDoctor(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Doctor>> GetDoctors()
        {
            return await _context.Doctors.ToListAsync();
        }

        public async Task<Doctor> GetDoctor(int id)
        {
            return await _context.Doctors.FindAsync(id);
        }
        
        public async Task UpdateDoctor(int id, Doctor doctor)
        {
            var doctorExist = await _context.Doctors.FindAsync(id);
            if (doctorExist != null)
            {
                doctorExist.Email = doctor.Email;
                doctorExist.FirstName = doctor.FirstName;
                _context.Doctors.Update(doctorExist);
                await _context.SaveChangesAsync();
            }else
            {
                throw new Exception("Doctor not found");
            }
        }
        public async Task DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }else
            {
                throw new Exception("Doctor not found");
            }
        }
        
        
        public async Task<PrescriptionDto> GetPrescription(int id)
        {
            var wantedPrescription = await _context.Prescriptions.FindAsync(id);

            if (wantedPrescription == null)
            {
                throw new Exception("Prescription not found");
            }
            
            var prescriptionDto = await _context
                .Prescriptions
                .Where(e => e.IdPrescription == id)
                .Select(e => new PrescriptionDto
                {
                    Date = e.Date,
                    DueDate = e.DueDate,
                    Doctor = e.Doctor,
                    Patient = e.Patient,
                    Medicaments = e.PrescriptionMedicaments.Select(x => new MedicamentDto
                    {
                        IdMedicament = x.Medicament.IdMedicament,
                        Name = x.Medicament.Name,
                        Description = x.Medicament.Description,
                        Type = x.Medicament.Type,
                        Dose = x.Dose,
                        Details = x.Details
                    }).ToList()
                }).FirstAsync();
        
            return prescriptionDto;
        }
    }
}