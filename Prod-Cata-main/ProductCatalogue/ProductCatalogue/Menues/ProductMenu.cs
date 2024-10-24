using Resources.Models;
using Resources.Services;
using System.Runtime.CompilerServices;

namespace ProductCatalogue.Menues;

internal class ProductMenu
{
    private readonly ProductService _productService;
    
    public ProductMenu(ProductService productService)
    {
        _productService = productService;
    }

    public void CreateProductMenu()
    {
        Console.Clear();
        Console.WriteLine("Create new product; ");

        string productName = SetProductName();
        decimal productPrice = SetProductPrice();
        string productId = Guid.NewGuid().ToString();

        Product product = new Product(productId, productName, productPrice);
        var result = _productService.AddToList(product);

        if (result.Succeeded)
        {
            Console.WriteLine($"{result.Message}");
        }
        else
        {
            Console.WriteLine($"{result.Message}");
        }
    }

    public void ShowProductsMenu(int number)
    {
        Console.Clear();
        var result = _productService.GetAllProducts();
        IEnumerable<Product>? content = result.Content;

        string flatLine = new string('-', 40);

        if (result.Succeeded && content != null)
        {
            Console.WriteLine(flatLine);
            Console.WriteLine("Product list; ");
            Console.WriteLine(flatLine);
            foreach (Product product in content) 
            {
                if (number == 1)
                {
                    Console.WriteLine($"{product.ProductName} | {product.ProductPrice} ");
                }
                else
                {
                    Console.WriteLine($"{product.ProductName} | {product.ProductId}");
                }
            }
            Console.WriteLine(flatLine);
        }
        else
        {
            Console.WriteLine("Something went wrong with the list.");
        }
    }

    public void UpdateProductMenu()
    {
        Console.WriteLine("** Update product; **");
        Console.Write("\nWhat is the products id? > ");
        string updateId = (Console.ReadLine() ?? "");
        var productToUpdate = _productService.GetAProduct(updateId);
        if (productToUpdate == null)
        {
            Console.WriteLine("Must input a valid id.");
            return;
        }
        Console.Write("\nWhich would you like to update? (Name or price) > ");
        string choice = (Console.ReadLine() ?? "").ToUpper();

        if (choice == "PRICE")
        {
            UpdatePrice(productToUpdate);
        }
        else if (choice == "NAME")
        {
            UpdateName(productToUpdate);
        }
        else
        {
            Console.WriteLine("Must input a valid choice.");
        }
    }

    public void UpdatePrice(Product productToUpdate)
    {
        decimal newProductPrice = SetProductPrice();
        string oldProductName = productToUpdate.ProductName;
        string oldId = productToUpdate.ProductId;
        var result = _productService.UpdateProduct(oldId, oldProductName, newProductPrice);

        if (result.Succeeded)
        {
            Console.Clear();
            Console.WriteLine($"{result.Message}");
        }
        else
        {
            Console.Clear();
            Console.WriteLine($"{result.Message}");
        }
    }
    public void UpdateName(Product productToUpdate)
    {
        string newProductName = SetProductName();
        decimal oldProductPrice = productToUpdate.ProductPrice;
        string oldId = productToUpdate.ProductId;
        var result = _productService.UpdateProduct(oldId, newProductName, oldProductPrice);

        if (result.Succeeded)
        {
            Console.Clear();
            Console.WriteLine($"{result.Message}");
        }
        else
        {
            Console.Clear();
            Console.WriteLine($"{result.Message}");
        }
    }

    public void DeleteProductMenu() 
    {
        Console.WriteLine("** Delete product; **");
        Console.Write("\nWhat is the products id? > ");
        string idDelete = (Console.ReadLine() ?? "");
        if (string.IsNullOrEmpty(idDelete))
        {
            Console.WriteLine("Invalid id.");
        }
        var result = _productService.DeleteProduct(idDelete);
        if (result.Succeeded)
        {
            Console.Clear();
            Console.WriteLine($"{result.Message}");
        }
        else
        {
            Console.Clear();
            Console.WriteLine($"{result.Message}");
        }
    }

    public string SetProductName()
    {
        Console.Write("Product name > ");
        string productName = Console.ReadLine() ?? "";
        while (string.IsNullOrEmpty(productName))
        {
            Console.WriteLine("Invalid input.");
            Console.Write("Please add a product name; > ");
            productName = Console.ReadLine() ?? "";
        }
        return productName;
    }

    public decimal SetProductPrice()
    {
        Console.Write("Product price > ");
        string productPriceString = Console.ReadLine() ?? "";
        decimal productPrice;
        while (!decimal.TryParse(productPriceString, out productPrice))
        {
            Console.WriteLine("Invalid price. Must enter a valid decimal number.");
            Console.Write("Enter a new price; ");
            productPriceString = Console.ReadLine() ?? "";
        }
        return productPrice;
    }
}
