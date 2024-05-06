using Task6.Models;

namespace Task6.Services;

public interface IProduct_WarehouseService
{
    IEnumerable<Product_Warehouse> GetProductWarehouse();
    public int InsertProduct(Product_Warehouse productWarehouse);
    public bool IsAmountCorrect(int amount);


}