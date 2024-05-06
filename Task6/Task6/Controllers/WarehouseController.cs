using Microsoft.AspNetCore.Mvc;
using Task6.Services;
using Task6.Models;
namespace Task6.Controllers;


[ApiController]
[Route("api/[controller]")]
public class WarehouseController: ControllerBase
{
    private IProduct_WarehouseService _productWarehouseService;
    private ILogger<WarehouseController> _logger;

    public WarehouseController(IProduct_WarehouseService productWarehouseService, ILogger<WarehouseController> logger)
    {
        _productWarehouseService = productWarehouseService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetProduct_Warehouse()
    {
        var warehouse = _productWarehouseService.GetProductWarehouse();
        return Ok(warehouse);
    }


    [HttpPut]
    public IActionResult InsertIntoProductWarehouse([FromBody] Product_Warehouse productWarehouse)
    {
        try
        {
            int result = _productWarehouseService.InsertProduct(productWarehouse);
            if (result == -1)
            {
                return BadRequest("Invalid data provided.");
            }
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid input data.");
            return BadRequest(ex.Message);
        }
    }
    

    

}