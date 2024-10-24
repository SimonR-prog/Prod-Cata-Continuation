using Resources.Models;

namespace Resources.Interface
{
    public interface IProductService
    {
        Response<Product> AddToList(Product product);
        Response<Product> DeleteProduct(string id);
        Response<IEnumerable<Product>> GetAllProducts();
        Product GetAProduct(string id);
        Response<Product> UpdateProduct(string oldId, string newProductName, decimal newProductPrice);
        bool ValidProduct(string productId, string productName, decimal productPrice);
    }
}