using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTOs;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(WarehouseProductDTO productDto)
    {
        bool productExists = await _warehouseService.ProductExists(productDto.IdProduct);
        if (!productExists)
        {
            return NotFound($"Nie można odnaleźć produktu o id: {productDto.IdProduct}");
        }

        bool warehouseExists = await _warehouseService.WarehouseExists(productDto.IdWarehouse);
        if (!warehouseExists)
        {
            return NotFound($"Nie można odnaleźć magazynu o id: {productDto.IdWarehouse}");
        }

        int idOrder =
            await _warehouseService.OrderVerification(productDto.IdProduct, productDto.Amount, productDto.CreatedAt);
        if (idOrder == Int32.MinValue)
        {
            return NotFound("Nie można znaleźć takiego zamówienia");
        }

        bool notCompleted = await _warehouseService.NotCompleted(idOrder);
        if (!notCompleted)
        {
            return NotFound($"Zamówienie o id: {idOrder} zostało skompletowane");
        }

        await _warehouseService.UpdateFullfilledAt(idOrder);
        await _warehouseService.InsertToProductWarehouse(productDto, idOrder);

        int pk = await _warehouseService.GetPrimaryKey(productDto, idOrder);
        if (pk == 0)
        {
            return NotFound("Nie można znaleźć podanego zamówienia");
        }

        return Created();
    }
}