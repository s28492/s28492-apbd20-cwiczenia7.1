
using Task6.Models;

namespace Task6.Repositories;

public interface IProduct_WarehouseRepository
{
    IEnumerable<Product_Warehouse> GetProduct_Warehouse();
    public bool DoesProductExists(int id);
    public decimal GetProductPrice(int id);
    public bool DoesWarehouseExists(int id);
    public bool DoesOrderExists(int id, int amount, DateTime date);
    public int GetFirstUnfulfilledOrderId(int productId, int amount, DateTime beforeDate);
    
    public bool AddFulfilledAt(int id, int amount);
    public bool IsOrderAlreadyInProductWarehouse(int idProduct, int idWarehouse, int amount, DateTime date);
    public bool InsertIntoProductWarehouse(Product_Warehouse productWarehouse, int idOrder, Decimal price);
    public int GetLastAddedProductWarehouse();
}