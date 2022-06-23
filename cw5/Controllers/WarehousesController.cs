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
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _service;

        public WarehousesController(IWarehouseService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> postWarehouse(WarehousePost warehousePost)
        {
            if (warehousePost.Amount <= 0)
            {
                return Conflict("Amount musi być wieksze 0");
            }

            if (!await _service.DoesProductExist(warehousePost.IdProduct) ||
                !await _service.DoesWarehouseExist(warehousePost.IdWarehouse))
            {
                return NotFound("Produkt lub magazyn nie istnieje");
            }


            var orderId = await _service.GetTheValidOrderId(warehousePost.IdWarehouse, warehousePost.IdProduct,
                warehousePost.Amount, warehousePost.CreatedAt);

            if (orderId < 0)
                return NotFound("nie ma zamówienmia");
            
            if (await _service.WasOrderFullfilled(orderId))
            {
                return BadRequest("There is already an order in this warehouse");
            }

            if (!await _service.CompeleteTheOrder(orderId, warehousePost.IdWarehouse, warehousePost.IdProduct,
                warehousePost.Amount, warehousePost.CreatedAt))
            {
                return Conflict("Nie udało się zrealizować zamówienia");
            }

            int result = await _service.AddProductToWarehouse(warehousePost.IdWarehouse, warehousePost.IdProduct,
                warehousePost.Amount, warehousePost.IdProduct);

            return Created(result.ToString(), warehousePost);
        }
        
        [HttpPost("2")]
        public async Task<IActionResult> postWarehouse2([FromBody] WarehousePost warehouse)
        {
            int result = await _service.StoredProcedure(warehouse.IdWarehouse, warehouse.IdProduct, warehouse.Amount, DateTime.Now);

            return Created(result.ToString(), warehouse);

        }
    }
}