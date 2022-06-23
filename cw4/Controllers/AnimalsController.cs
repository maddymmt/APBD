using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw4.Models;
using cw4.Services;
using Microsoft.AspNetCore.Mvc;

namespace cw4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {

        private readonly IAnimalService _service;
        public AnimalsController(IAnimalService service){
            _service=service;
        }


        [HttpGet]
        public IActionResult GetAnimals (string orderBy="name"){
            return Ok(_service.GetAnimals(orderBy));
        }

        [HttpPost]
        public IActionResult AddAnimal(Animal animal){
            _service.AddAnimal(animal);
            return Created("","");
        }

        [HttpPut("{IdAnimal}")]
        public IActionResult UpdateAnimal(Animal animal, int idAnimal)
        {
            _service.UpdateAnimal(animal, idAnimal);
            return Ok();
        }
        
        [HttpDelete("{IdAnimal}")]
        public IActionResult DeleteAnimal(Animal animal, int idAnimal)
        {
            _service.DeleteAnimal(animal, idAnimal);
            return Ok();
        }
    }
}