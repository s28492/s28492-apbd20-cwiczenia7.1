using Microsoft.Data.SqlClient;
using Task6.Models;

namespace Task6.Repositories;

public class Product_WarehouseRepository : IProduct_WarehouseRepository
{
    private IConfiguration _configuration;

    public Product_WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<Product_Warehouse> GetProduct_Warehouse()
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand("SELECT * FROM Product_Warehouse", con);
        using var dr = cmd.ExecuteReader();
        var warehouses = new List<Product_Warehouse>();
        while (dr.Read())
        {
            warehouses.Add(new Product_Warehouse
            {
                IdWarehouse = (int)dr["IdWarehouse"],
                IdProduct = (int)dr["IdProduct"],
                Amount = (int)dr["Amount"],
                CreatedAt = (DateTime)dr["CreatedAt"]
            });
        }
        return warehouses;
    }

    public bool DoesProductExists(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand("SELECT IdProduct FROM Product WHERE IdProduct = @productId", con);
        cmd.Parameters.AddWithValue("@productId", id);
        using var dr = cmd.ExecuteReader();
        return dr.Read();
    }
    public decimal GetProductPrice(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand("SELECT Price FROM Product WHERE IdProduct = @productId", con);
        cmd.Parameters.AddWithValue("@productId", id);

        var result = cmd.ExecuteScalar();
        if (result != null)
            return (decimal)result;

        throw new InvalidOperationException("Product not found or price is null.");
    }

    public bool DoesWarehouseExists(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand("SELECT IdWarehouse FROM Warehouse WHERE IdWarehouse = @warehouseId", con);
        cmd.Parameters.AddWithValue("@warehouseId", id);
        using var dr = cmd.ExecuteReader();
        return dr.Read();
    }
    
    public bool DoesOrderExists(int id, int amount, DateTime date)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand("SELECT TOP 1 * FROM [Order] WHERE IdProduct = @idProduct AND Amount = @amount AND FulfilledAt IS NULL AND CreatedAt < @addingDate ORDER BY CreatedAt ASC;", con);
        cmd.Parameters.AddWithValue("@idProduct", id);
        cmd.Parameters.AddWithValue("@amount", amount);
        cmd.Parameters.AddWithValue("@addingDate", date);
        using var dr = cmd.ExecuteReader();

        return dr.Read();

    }
    public int GetFirstUnfulfilledOrderId(int productId, int amount, DateTime addingDate)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand(
            "SELECT TOP 1 IdOrder FROM [Order] " +
            "WHERE IdProduct = @productId AND Amount = @amount AND FulfilledAt is Null AND CreatedAt < @addingDate " +
            "ORDER BY CreatedAt ASC", con);

        cmd.Parameters.AddWithValue("@productId", productId);
        cmd.Parameters.AddWithValue("@amount", amount);
        cmd.Parameters.AddWithValue("@addingDate", addingDate);

        var result = cmd.ExecuteScalar();
        if (result != null)
            return (int)result;

        throw new InvalidOperationException("No matching order found.");
    }
    public bool IsOrderAlreadyInProductWarehouse(int idProduct, int idWarehouse, int amount, DateTime date)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand("SELECT 1 FROM Product_Warehouse WHERE IdWarehouse = @idWarehouse AND IdProduct = @idProduct AND Amount = @amount AND CreatedAt = @addingDate;", con);
        cmd.Parameters.AddWithValue("@idWarehouse", idWarehouse);
        cmd.Parameters.AddWithValue("@idProduct", idProduct);
        cmd.Parameters.AddWithValue("@amount", amount);
        cmd.Parameters.AddWithValue("@addingDate", date);
        using var dr = cmd.ExecuteReader();
        
        return dr.Read();

    }
    public bool AddFulfilledAt(int id, int amount)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand("Update [Order] Set FulfilledAt = @addingDate Where IdOrder = (SELECT TOP 1 IdOrder FROM [Order] WHERE IdProduct = @idProduct AND Amount = @amount AND FulfilledAt IS NULL AND CreatedAt < @addingDate ORDER BY CreatedAt ASC);", con);
        cmd.Parameters.AddWithValue("@idProduct", id);
        cmd.Parameters.AddWithValue("@amount", amount);
        cmd.Parameters.AddWithValue("@addingDate", DateTime.Now);
        using var dr = cmd.ExecuteReader();
        
        return true;

    }

    public bool InsertIntoProductWarehouse(Product_Warehouse productWarehouse, int idOrder, Decimal price)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand("INSERT INTO Product_Warehouse  VALUES (@idWarehouse, @idProduct, @idOrder, @amount, @price, @createdAt);", con);
        cmd.Parameters.AddWithValue("@idWarehouse", productWarehouse.IdWarehouse);
        cmd.Parameters.AddWithValue("@idProduct", productWarehouse.IdProduct);
        cmd.Parameters.AddWithValue("@idOrder", idOrder);
        cmd.Parameters.AddWithValue("@amount", productWarehouse.Amount);
        cmd.Parameters.AddWithValue("@price", price);
        cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);
        using var dr = cmd.ExecuteReader();
        
        return true;
    }
    public int GetLastAddedProductWarehouse()
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand("Select TOP 1 IdProductWarehouse From Product_Warehouse ORDER BY IdProductWarehouse DESC", con);
        
        var result = cmd.ExecuteScalar();
        if (result != null)
            return (int)result;

        throw new InvalidOperationException("No products in warehouse found.");
    }
    
    
}