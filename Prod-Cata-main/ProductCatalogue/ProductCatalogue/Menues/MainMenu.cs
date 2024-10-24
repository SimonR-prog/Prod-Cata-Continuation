namespace ProductCatalogue.Menues;

internal class MainMenu
{
    private readonly ProductMenu _productMenu;

    //A constructor that initializes the productmenu into the private field above.
    public MainMenu(ProductMenu productMenu)
    {
        _productMenu = productMenu;
    }

    
    public void StartMenu()
    {   
        //Method to show the different options that the user can do in the first menu.
        Console.Clear();
        Console.WriteLine("Menu;");
        Console.WriteLine("1. Create a product.");
        Console.WriteLine("2. Show product list.");
        Console.WriteLine("3. Remove a product.");
        Console.WriteLine("4. Update a product.");
        Console.WriteLine("0. Exit program.");
        Console.Write("Enter an option; ");

        //Storing whichever choice the user types into the choice variable.
        var choice = Console.ReadLine();

        //Switch which will check the choice input across all the different possible cases at the same time and send the user on to the next menu.
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
                //If the user picks 0, the program will end.
                Environment.Exit(0);
                break;
            default:
                //Default to catch the times to user put something that doesn't fit into the switch cases.
                Console.WriteLine("\n> Invalid input.");
                break;
        }
    }
}
