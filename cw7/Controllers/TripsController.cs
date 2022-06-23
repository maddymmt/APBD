using System;
using System.Threading.Tasks;
using cw7.Models;
using cw7.Models.DTOs;
using cw7.Services;
using Microsoft.AspNetCore.Mvc;

namespace cw7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _service;

        public TripsController(ITripService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            try
            {
                return Ok(await _service.GetTrips());
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        [HttpDelete("clients/{idClient}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            await _service.DeleteClient(id);
            return NoContent();
        }
        
        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddTripToClient(ClientTripDto clientTripDto)
        {
            await _service.AddTripToClient(clientTripDto);
            return Ok();
        }
    }
}