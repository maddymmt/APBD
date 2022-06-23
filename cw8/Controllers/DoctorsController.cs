using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw8.Models;
using cw8.Models.DTOs;
using cw8.Services;
using Microsoft.AspNetCore.Mvc;

namespace cw8.Controllers
{
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IMedservice _service;
        public DoctorsController(IMedservice service)
        {
            _service = service;
        }
        
        [HttpPost]
        public async Task<IActionResult> AddDoctor(DoctorPost data)
        {
           await  _service.AddDoctor(new Models.Doctor()
           {
               FirstName = data.FirstName,
               LastName = data.LastName,
               Email = data.Email
               
           });

           return Created("",data);
        }
        
        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _service.GetDoctors();
            return Ok(doctors);
        }
        [HttpGet("doctors/{id}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            
            var doctor = await _service.GetDoctor(id);
            return Ok(doctor);
        }
        [HttpGet("prescriptions/{id}")]
        public async Task<IActionResult> GetPrescriptions(int id)
        {
            var prescription = await _service.GetPrescription(id);
            return Ok(prescription);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, DoctorPut data)
        {
            await _service.UpdateDoctor(id, new Models.Doctor()
            {
                
                Email = data.Email,
                FirstName = data.FirstName
            });
            return Ok(data);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            await _service.DeleteDoctor(id);
            return Ok("Doctor deleted");
        }
        
    }
}