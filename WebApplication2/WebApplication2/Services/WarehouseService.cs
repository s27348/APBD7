using WebApplication2.DTOs;
using WebApplication2.Repositories;

namespace WebApplication2.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<bool> ProductExists(int idProduct)
    {
        return await _warehouseRepository.ProductExists(idProduct);
    }

    public async Task<bool> WarehouseExists(int idWarehouse)
    {
        return await _warehouseRepository.WarehouseExists(idWarehouse);
    }

    public async Task<int> OrderVerification(int idProduct, int amount, DateTime createdAt)
    {
        return await _warehouseRepository.OrderVerification(idProduct, amount, createdAt);
    }

    public async Task<bool> NotCompleted(int idOrder)
    {
        return await _warehouseRepository.NotCompleted(idOrder);
    }

    public async Task<int> UpdateFullfilledAt(int idOrder)
    {
        return await _warehouseRepository.UpdateFullfilledAt(idOrder);
    }

    public async Task<decimal> GetPrice(int idProduct)
    {
        return await _warehouseRepository.GetPrice(idProduct);
    }

    public async Task<int> InsertToProductWarehouse(WarehouseProductDTO productDto, int idOrder)
    {
        return await _warehouseRepository.InsertToProductWarehouse(productDto, idOrder);
    }

    public async Task<int> GetPrimaryKey(WarehouseProductDTO productDto, int idOrder)
    {
        return await _warehouseRepository.GetPrimaryKey(productDto, idOrder);
    }
}