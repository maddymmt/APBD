using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw8.Models;
using cw8.Models.DTOs;

namespace cw8.Services
{
    public interface IMedservice
    {

        public Task AddDoctor(Doctor doctor);
        public Task<List<Doctor>> GetDoctors();
        public Task<Doctor> GetDoctor(int id);
        
        public Task  UpdateDoctor(int id, Doctor doctor);
        public Task DeleteDoctor(int id);
        public Task<PrescriptionDto> GetPrescription(int id);

    }
}