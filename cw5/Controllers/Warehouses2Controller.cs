using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using cw5.Models;
using cw5.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace cw5.Controllers
{
    [Route("[controller]")]
    public class Warehouses2Controller : ControllerBase
    {
        private readonly IWarehouseService _service;

        public Warehouses2Controller(IWarehouseService service)
        {
            _service = service;
        }
        
        [HttpPost]
        public IActionResult AddWarehouse(WarehousePost warehouse)
        {
            _service.AddWarehouse(warehouse);
            return Ok();
        }
    }
}