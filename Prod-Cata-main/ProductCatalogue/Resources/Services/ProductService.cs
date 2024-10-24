using Newtonsoft.Json;
using Resources.Interface;
using Resources.Models;


namespace Resources.Services;

public class ProductService : IProductService
{
    private readonly IFileService _fileService;
    private List<Product> _products;

    public ProductService(IFileService fileService)
    {
        _fileService = fileService;
        _products = [];
        GetAllProducts();
    }

    public Response<Product> AddToList(Product product)
    {
        if (!ValidProduct(product.ProductId, product.ProductName, product.ProductPrice))
        {
            return new Response<Product>
            {
                Succeeded = false,
                Message = $"Productname; {product.ProductName} \nProductprice; {product.ProductPrice} \nProductid; {product.ProductId}\nOne of the above parameters are invalid."
            };
        }

        if (_products.Any(pName => string.Equals(pName.ProductName, product.ProductName, StringComparison.OrdinalIgnoreCase)))
        {
            return new Response<Product>
            {
                Succeeded = false,
                Message = $"{product.ProductName} is already on the list."
            };
        }
        try
        {
            _products.Add(product);

            var json = JsonConvert.SerializeObject(_products, Formatting.Indented);
            var result = _fileService.SaveToFile(json);

            if (result.Succeeded)
            {
                return new Response<Product>
                {
                    Succeeded = true,
                    Message = $"{product.ProductName} was added."
                };
            }
            return new Response<Product>
            {
                Succeeded = false,
                Message = $"{product.ProductName} was not added."
            };
        }
        catch (Exception ex)
        {
            return new Response<Product>
            {
                Succeeded = false,
                Message = ex.Message
            };
        }
    }
    
    public Response<IEnumerable<Product>> GetAllProducts()
    {
        try
        {
            var result = _fileService.GetFromFile();

            if (result.Succeeded)
            {
                _products = JsonConvert.DeserializeObject<List<Product>>(result.Content!)!;
                return new Response<IEnumerable<Product>>
                {
                    Succeeded = true,
                    Content = _products
                };
            }
            else
            {
                return new Response<IEnumerable<Product>>
                {
                    Succeeded = false,
                    Message = "Something went wrong with getting the list."
                };
            }
        }
        catch (Exception ex)
        {
            return new Response<IEnumerable<Product>>
            { 
                Succeeded = false, 
                Message = ex.Message 
            };
        }
    }

    public Response<Product> DeleteProduct(string id)
    {
        if (string.IsNullOrEmpty(id))
            return new Response<Product>
            {
                Succeeded = false,
                Message = "Must input a valid id."
            };
        var productToRemove = _products.ToList().FirstOrDefault(p => p.ProductId == id);
        if (productToRemove == null)
        {
            return new Response<Product>
            {
                Succeeded = false,
                Message = "Product does not exist."
            };
        }
        try
        {
            _products.Remove(productToRemove);

            var json = JsonConvert.SerializeObject(_products, Formatting.Indented);
            var result = _fileService.SaveToFile(json);
            if (result.Succeeded)
            {
                return new Response<Product>
                {
                    Succeeded = true,
                    Message = $"{productToRemove.ProductName} was removed."
                };
            }
            else 
            { 
                return new Response<Product> 
                { 
                    Succeeded = false, 
                    Message = result.Message 
                }; 
            }
        }
        catch (Exception ex)
        {
            return new Response<Product>
            {
                Succeeded = false,
                Message = ex.Message
            };
        }
    }
    
    public Response<Product> UpdateProduct(string oldId, string newProductName, decimal newProductPrice)
    {

        if (!ValidProduct(oldId, newProductName, newProductPrice))
        {
            Console.WriteLine("Cunted.");
            return new Response<Product>
            {
                Succeeded = false,
                Message = "Something went wrong."
            };
        }
        var remove = GetAProduct(oldId);
        _products.Remove(remove);
        Product product = new Product(oldId, newProductName, newProductPrice);
        var result = AddToList(product);

        if (result.Succeeded)
        {
            return new Response<Product>
            {
                Succeeded = true,
                Message = $"{product.ProductName} was updated."
            };
        }
        return new Response<Product>
        {
            Succeeded = false,
            Message = $"{product.ProductName} was not updated."
        };
    }

    public bool ValidProduct(string productId, string productName, decimal productPrice)
    {
        if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(productId) || productPrice < 0m)
            return false;
        return true;
    }

    public Product GetAProduct(string id)
    {
        var product = _products.ToList().FirstOrDefault(p => p.ProductId == id);
        if (product != null)
            return product;
        else
            return null!;
    }
}
