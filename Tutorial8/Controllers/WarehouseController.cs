using Microsoft.AspNetCore.Mvc;
using Tutorial8.Contracts.Requests;
using Tutorial8.Entities;
using Tutorial8.Repositories.Interfaces;

namespace Tutorial8.Controllers;

[ApiController]
[Route("api/warehouses")]
public class WarehouseController(IWarehouseRepository warehouseRepository, IOrderRepository orderRepository) : ControllerBase
{
    
    [HttpGet("test-connection")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> TestConnection(CancellationToken ct)
    {
        var result = await warehouseRepository.TestConnection(ct);
        if (result)
        {
            return Ok("Connection successful");
        }
        return StatusCode(StatusCodes.Status500InternalServerError, "Connection failed");
    }
    
    [HttpPost("add-product")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddProductAsync([FromBody] ProductWarehouseRequest product, CancellationToken ct)
    {
        Console.WriteLine(product);
        if (product.IdProduct < 0) return BadRequest("Product id must be greater than 0");
        
        if (product.IdWarehouse < 0) return BadRequest("Warehouse id must be greater than 0");
        
        var orderInfo = await orderRepository.OrderExistsWithProductId(product.IdProduct, product.Amount, DateTime.Parse(product.CreatedAt), ct);
        
        if (!orderInfo.exists)
        {
            return BadRequest("Order that requires this product in this amount does not exist");
        }

        if (await orderRepository.IsOrderFulfilled(orderInfo.orderId, ct))
        {
            return BadRequest("Order that requires this product in this amount has already been fulfilled");
        }
        
        await orderRepository.UpdateOrderFulfilled(orderInfo.orderId, ct);
        
        var productWarehouse = new ProductWarehouse
        {
            IdProduct = product.IdProduct,
            IdWarehouse = product.IdWarehouse,
            Amount = product.Amount
        };
        
        var result = await warehouseRepository.AddProductToWarehouse(productWarehouse, orderInfo.orderId, ct);
        
        if (result != -1)
        {
            return CreatedAtAction(nameof(AddProductAsync), new { id = result }, result);
        }
        
        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add product to warehouse");
    } 
}