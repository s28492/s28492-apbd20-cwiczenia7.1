using Task6.Models;
using Task6.Repositories;
namespace Task6.Services;

public class Product_WarehouseService : IProduct_WarehouseService
{
    private readonly IProduct_WarehouseRepository _product_WarehouseRepository;

    public Product_WarehouseService(IProduct_WarehouseRepository productWarehouseRepository)
    {
        _product_WarehouseRepository = productWarehouseRepository;
    }


    public IEnumerable<Product_Warehouse> GetProductWarehouse()
    {
        return _product_WarehouseRepository.GetProduct_Warehouse();
    }

    public int InsertProduct(Product_Warehouse productWarehouse)
    {
        try
        {
            if (!IsAmountCorrect(productWarehouse.Amount))
                return -1; 

            if (!_product_WarehouseRepository.DoesProductExists(productWarehouse.IdProduct) ||
                !_product_WarehouseRepository.DoesWarehouseExists(productWarehouse.IdWarehouse))
                return -1; 

            int orderId = _product_WarehouseRepository.GetFirstUnfulfilledOrderId(
                productWarehouse.IdProduct, productWarehouse.Amount, productWarehouse.CreatedAt);

            if (orderId == -1)
                return -1; 

            decimal price = _product_WarehouseRepository.GetProductPrice(productWarehouse.IdProduct);
            _product_WarehouseRepository.InsertIntoProductWarehouse(productWarehouse, orderId, price);

            return _product_WarehouseRepository.GetLastAddedProductWarehouse();
        }
        catch (Exception)
        {
            throw; // Re-throw the exception to be handled by the caller
        }
    }

    public bool IsAmountCorrect(int amount)
    {
        return amount > 0;
    }
}