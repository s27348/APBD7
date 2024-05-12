using System.Data.SqlClient;
using WebApplication2.DTOs;

namespace WebApplication2.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<bool> ProductExists(int idProduct)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Product WHERE IdProduct = @IdProduct";
        cmd.Parameters.AddWithValue("@IdProduct", idProduct);

        if (await cmd.ExecuteScalarAsync() is not null)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> WarehouseExists(int idWarehouse)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Warehouse WHERE IdWarehouse = @IdWarehouse";
        cmd.Parameters.AddWithValue("@IdWarehouse", idWarehouse);

        if (await cmd.ExecuteScalarAsync() is not null)
        {
            return true;
        }
        return false;
    }

    public async Task<int> OrderVerification(int idProduct, int amount, DateTime createdAt)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText =
            "SELECT IdOrder FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount AND CreatedAt < @CreatedAt";
        cmd.Parameters.AddWithValue("@IdProduct", idProduct);
        cmd.Parameters.AddWithValue("@Amount", amount);
        cmd.Parameters.AddWithValue("@CreatedAt", createdAt);

        var result = await cmd.ExecuteScalarAsync();
        if (result is not null)
        {
            return (int)result;
        }
        return Int32.MinValue;
    }

    public async Task<bool> NotCompleted(int idOrder)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText =
            "SELECT IdOrder FROM Product_Warehouse WHERE IdOrder = @IdOrder";
        cmd.Parameters.AddWithValue("@IdOrder", idOrder);
        
        var result = await cmd.ExecuteScalarAsync();
        
        if (result is not null)
        {
            return false;
        }
        return true;
    }

    public async Task<int> UpdateFullfilledAt(int idOrder)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText =
            "UPDATE [Order] SET FullfilledAt = @Currdate WHERE IdOrder = @IdOrder";
        cmd.Parameters.AddWithValue("@Currdate", DateTime.Now);
        cmd.Parameters.AddWithValue("@IdOrder", idOrder);
        
        var result = await cmd.ExecuteNonQueryAsync();

        return result;
    }

    public async Task<decimal> GetPrice(int idProduct)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText =
            "SELECT Price FROM Product WHERE IdProduct = @IdProduct";
        cmd.Parameters.AddWithValue("@IdProduct", idProduct);
        
        var result = await cmd.ExecuteScalarAsync();
        
        if (result is not null)
        {
            return (decimal)result;
        }
        return 0;
    }

    public async Task<int> InsertToProductWarehouse(WarehouseProductDTO productDto, int idOrder)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        decimal cost = await GetPrice(productDto.IdProduct);
        decimal price = cost * productDto.Amount;
        cmd.Connection = con;
        cmd.CommandText =
            "INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)";
        cmd.Parameters.AddWithValue("@IdWarehouse", productDto.IdWarehouse);
        cmd.Parameters.AddWithValue("@IdProduct", productDto.IdProduct);
        cmd.Parameters.AddWithValue("@IdOrder", idOrder);
        cmd.Parameters.AddWithValue("@Amount", productDto.Amount);
        cmd.Parameters.AddWithValue("@Price", price);
        cmd.Parameters.AddWithValue("@CreatedAtRoute", productDto.CreatedAt);

        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<int> GetPrimaryKey(WarehouseProductDTO productDto, int idOrder)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText =
            "SELECT IdOrder FROM Product_Warehouse WHERE IdWarehouse = @IdWarehouse AND IdProduct = @IdProduct AND IdOrder = @IdOrder";
        cmd.Parameters.AddWithValue("IdWarehouse", productDto.IdWarehouse);
        cmd.Parameters.AddWithValue("@IdProduct", productDto.IdProduct);
        cmd.Parameters.AddWithValue("@IdOrder", idOrder);

        var result = await cmd.ExecuteScalarAsync();
        if (result is not null)
        {
            return (int)result;
        }

        return 0;
    }
}