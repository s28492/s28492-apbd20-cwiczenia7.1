using System.ComponentModel.DataAnnotations;

namespace Task6.Models;

public class Product_Warehouse
{
    [Required]
    public int IdProduct { get; set; }

    [Required]
    public int IdWarehouse { get; set; }
    
    [Required]
    public int Amount { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
}