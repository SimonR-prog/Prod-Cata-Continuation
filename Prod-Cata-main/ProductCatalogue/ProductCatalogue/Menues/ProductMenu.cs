using Resources.Models;
using Resources.Services;
using System.Runtime.CompilerServices;

namespace ProductCatalogue.Menues;

internal class ProductMenu
{
    //Creating a private field for the productservice.
    private readonly ProductService _productService;
    
    //Constructor to assign the productservice to the field.
    public ProductMenu(ProductService productService)
    {
        _productService = productService;
    }

    public void CreateProductMenu()
    {
        //Clear the console before writing out instructions.
        Console.Clear();
        Console.WriteLine("Create new product; ");

        //Getting the user input by sending the user to the different methods and assigning the values returned to the variables.
        string productName = SetProductName();
        decimal productPrice = SetProductPrice();

        //Setting the products unique ID with guid.
        string productId = Guid.NewGuid().ToString();

        //Creating a new productobject of the productclass by sending the variables to the productclass.
        Product product = new Product(productId, productName, productPrice);

        //Sending the product to the add to list method inside the productservice. Storing the return into the result variable.
        var result = _productService.AddToList(product);

        //Writing out a message depending the bool of the Succeeded property.
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
        //Get list from the getallproducts method and stores it in the result.content.
        var result = _productService.GetAllProducts();
        
        //Turn the list in result.content into an IEnumerable.
        IEnumerable<Product>? content = result.Content;

        //Creating a flat line for a nicer showing of the list in the console.
        string flatLine = new string('-', 40);

        //If the succeeded property is true and the list in content is NOT null.
        if (result.Succeeded && content != null)
        {
            Console.WriteLine(flatLine);
            Console.WriteLine("Product list; ");
            Console.WriteLine(flatLine);
            //Foreach loop to write out the entire list on product at the time.
            foreach (Product product in content) 
            {
                //Depending on which number is sent from the other menu, will write out different properties of the product.
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
        //If anything goes wrong, error message.
        else
        {
            Console.WriteLine("Something went wrong with the list.");
        }
    }

    public void UpdateProductMenu()
    {
        Console.WriteLine("** Update product; **");

        //Asks for an ID to the product the user wants to update and stores it in the updateId string.
        Console.Write("\nWhat is the products id? > ");
        string updateId = (Console.ReadLine() ?? "");

        //Sends the id to the getaproduct method which returns the product that has that Id. Stores it in the producttoupdate variable.
        var productToUpdate = _productService.GetAProduct(updateId);

        //If the product is null, gives error message.
        if (productToUpdate == null)
        {
            Console.WriteLine("Must input a valid id.");
            return;
        }

        //Asks if it's the name or price the user wants to update and stores it in the choice in uppercase.
        Console.Write("\nWhich would you like to update? (Name or price) > ");
        string choice = (Console.ReadLine() ?? "").ToUpper();

        //Sends the user on to the next method depending on choice and sends the product.
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
        //Takes in the product to update and fetches a new price/name and assigns the old values that will not be changed.
        decimal newProductPrice = SetProductPrice();
        string oldProductName = productToUpdate.ProductName;
        string oldId = productToUpdate.ProductId;

        //Sends the variables to the updateproduct method inside the productservice.
        var result = _productService.UpdateProduct(oldId, oldProductName, newProductPrice);

        //Depending on result writes out the message that was sent back in the response message property.
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
        //Takes in the product to update and fetches a new price/name and assigns the old values that will not be changed.
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

        //Asks for an ID to the product the user wants to remove and stores it in the idDelete string.
        Console.Write("\nWhat is the products id? > ");
        string idDelete = (Console.ReadLine() ?? "");

        //Check to make sure the id string is neither null or empty.
        if (string.IsNullOrEmpty(idDelete))
        {
            Console.WriteLine("Invalid id.");
            return;
        }
        //Sends the id to the deleteproduct method in the productservice and stores it in the result variable.
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
        //Method to set the productname;
        Console.Write("Product name > ");
        string productName = Console.ReadLine() ?? "";

        //If the string is null or empty, the user ends up in a while loop until they give a produtname that works.
        while (string.IsNullOrEmpty(productName))
        {
            Console.WriteLine("Invalid input.");
            Console.Write("Please add a product name; > ");
            productName = Console.ReadLine() ?? "";
        }
        //Sends the productname back.
        return productName;
    }

    public decimal SetProductPrice()
    {
        //Method to set the productprice;
        Console.Write("Product price > ");
        string productPriceString = Console.ReadLine() ?? "";
        decimal productPrice;
        //If the users input can't be parsed into a decimal into the productprice then they end up in a while loop until something is added that can be.
        while (!decimal.TryParse(productPriceString, out productPrice))
        {
            Console.WriteLine("Invalid price. Must enter a valid decimal number.");
            Console.Write("Enter a new price; ");
            productPriceString = Console.ReadLine() ?? "";
        }
        //Sends back the productprice as a decimal.
        return productPrice;
    }
}
