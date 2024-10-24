namespace Resources.Models;
public class Product
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }

    public Product(string productId, string productName, decimal productPrice)
    {
        ProductId = productId;
        ProductName = productName;
        ProductPrice = productPrice;
    }

}

