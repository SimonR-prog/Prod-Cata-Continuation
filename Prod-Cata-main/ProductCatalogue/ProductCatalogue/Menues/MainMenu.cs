namespace ProductCatalogue.Menues;

internal class MainMenu
{
    private readonly ProductMenu _productMenu;

    public MainMenu(ProductMenu productMenu)
    {
        _productMenu = productMenu;
    }

    public void StartMenu()
    {   
        Console.Clear();
        Console.WriteLine("Menu;");
        Console.WriteLine("1. Create a product.");
        Console.WriteLine("2. Show product list.");
        Console.WriteLine("3. Remove a product.");
        Console.WriteLine("4. Update a product.");
        Console.WriteLine("0. Exit program.");
        Console.Write("Enter an option; ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                _productMenu.CreateProductMenu();
                break;
            case "2":
                _productMenu.ShowProductsMenu(1);
                break;
            case "3":
                _productMenu.ShowProductsMenu(2);
                _productMenu.DeleteProductMenu();
                break;
            case "4":
                _productMenu.ShowProductsMenu(2);
                _productMenu.UpdateProductMenu();
                break;
            case "0":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("\n> Invalid input.");
                break;
        }
    }
}
