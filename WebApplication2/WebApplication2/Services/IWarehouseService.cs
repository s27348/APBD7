using WebApplication2.DTOs;

namespace WebApplication2.Services;

public interface IWarehouseService
{
    public Task<bool> ProductExists(int idProduct);
    public Task<bool> WarehouseExists(int idWarehouse);
    public Task<int> OrderVerification(int idProduct, int amount, DateTime createdAt);
    public Task<bool> NotCompleted(int idOrder);
    public Task<int> UpdateFullfilledAt(int idOrder);
    public Task<decimal> GetPrice(int idProduct);
    public Task<int> InsertToProductWarehouse(WarehouseProductDTO productDto, int idOrder);
    public Task<int> GetPrimaryKey(WarehouseProductDTO productDto, int idOrder);
}