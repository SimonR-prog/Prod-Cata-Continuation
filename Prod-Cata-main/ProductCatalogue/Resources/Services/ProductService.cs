using Newtonsoft.Json;
using Resources.Interface;
using Resources.Models;
namespace Resources.Services;

public class ProductService : IProductService
{
    //Making private fields of the fileserive and the _products list.
    private readonly IFileService _fileService;
    private List<Product> _products;

    public ProductService(IFileService fileService)
    {
        //Constructor to initialize the fileservice runs makes sure that the program has the latest version of the list everytime it starts.
        _fileService = fileService;
        _products = [];
        GetAllProducts();
    }

    public Response<IEnumerable<Product>> AddToList(Product product)
    {
        //Recieves a product object of the product class.
        //Checks if the parts of the product are good or not.
        if (InvalidProduct(product.ProductId, product.ProductName, product.ProductPrice))
        {
            return new Response<IEnumerable<Product>>
            {
                Succeeded = false,
                Message = $"Productname; {product.ProductName} \nProductprice; {product.ProductPrice} \nProductid; {product.ProductId}\nOne of the above parameters are invalid."
            };
        }
        //Checks if the product name already is in the list or not. Stringcomparison to ignore the capitalization of the name.
        if (_products.Any(pName => string.Equals(pName.ProductName, product.ProductName, StringComparison.OrdinalIgnoreCase)))
        {
            return new Response<IEnumerable<Product>>
            {
                Succeeded = false,
                Message = $"{product.ProductName} is already on the list."
            };
        }
        try
        {
            //Adds the product to the list.
            _products.Add(product);

            //Serializes the _products list in an indented format and stores it in the json variable as a string.
            var json = JsonConvert.SerializeObject(_products, Formatting.Indented);

            //Sends it to the savetofile method in the fileservice to write it to the json file.
            var result = _fileService.SaveToFile(json);

            if (result.Succeeded)
            {
                //Returning the list in content to make it so that this can be tested more.
                return new Response<IEnumerable<Product>>
                {
                    Succeeded = true,
                    Message = $"{product.ProductName} was added.",
                    Content = _products
                };
            }
            return new Response<IEnumerable<Product>>
            {
                Succeeded = false,
                Message = $"{product.ProductName} was not added.",
                Content = _products
            };
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
    
    public Response<IEnumerable<Product>> GetAllProducts()
    {
        try
        {
            //Fetches the json string and stores it in the result variable.
            var result = _fileService.GetFromFile();
            if (result.Succeeded)
            {
                //Deserializes the string that it gets from the fileservice into a list of product objects and stores it in _products using newtonsoft.
                _products = JsonConvert.DeserializeObject<List<Product>>(result.Content!)!;

                //Returns the _products list in the content.
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
        //Checks if the id that got sent is either null or empty.
        if (string.IsNullOrEmpty(id))
            return new Response<Product>
            {
                Succeeded = false,
                Message = "Must input a valid id."
            };
        //Fetches the product that the user wants to remove using the product id.
        var productToRemove = GetAProduct(id);
        //Checks so the product is not null.
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
            //Removes the product from the list.
            _products.Remove(productToRemove);

            //Serializes the _products list with a indented format with newtonsoft into a json string and saves it in the json variable.
            var json = JsonConvert.SerializeObject(_products, Formatting.Indented);
            
            //Sends the string to the savetofile method.
            var result = _fileService.SaveToFile(json);
            
            //Depending on which bool comes back i nthe succeeded property in the response class, will write out a message.
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
        try
        {
            //Checks to make sure the variables are not invalid.
            if (InvalidProduct(oldId, newProductName, newProductPrice))
            {
                return new Response<Product>
                {
                    Succeeded = false,
                    Message = "Something went wrong."
                };
            }
            //Fetches to product using the id.
            var remove = GetAProduct(oldId);

            //Removes it from the list.
            _products.Remove(remove);

            //Creates a new product with the new name/price and the old id and send it to the addtolist method.
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
        catch (Exception ex)
        {
            return new Response<Product>
            {
                Message = ex.Message
            };
        }
            
    }

    public bool InvalidProduct(string productId, string productName, decimal productPrice)
    {
        //Takes in the different variables and check to make sure the strings are neither null or empty and that the price is not below 0.
        if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(productId) || productPrice < 0m)
            return true;
        return false;
    }

    public Product GetAProduct(string id)
    {
        //Uses the id it recieves to find the first product in the list that has the same id.
        var product = _products.FirstOrDefault(p => p.ProductId == id);
        //If the product is not null return the product, otherwise return null.
        if (product != null)
            return product;
        else
            return null!;
    }
}
