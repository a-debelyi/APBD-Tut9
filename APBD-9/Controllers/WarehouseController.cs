using APBD_9.DTOs;
using APBD_9.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IProductsWarehouseService _productsWarehouseService;

        public WarehouseController(IProductsWarehouseService productsWarehouseService)
        {
            _productsWarehouseService = productsWarehouseService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToWarehouseAsync(ProductWarehouseDto productWarehouseDto)
        {
            var productWarehouseId = await _productsWarehouseService.AddProductToWarehouseAsync(productWarehouseDto);
            return StatusCode(StatusCodes.Status201Created, new { id = productWarehouseId });
        }

        [HttpPost("procedure")]
        public async Task<IActionResult> AddProductToWarehouseViaProcedureAsync(ProductWarehouseDto productWarehouseDto)
        {
            var productWarehouseId =
                await _productsWarehouseService.AddProductToWarehouseViaProcedureAsync(productWarehouseDto);
            return StatusCode(StatusCodes.Status201Created, new { id = productWarehouseId });
        }
    }
}