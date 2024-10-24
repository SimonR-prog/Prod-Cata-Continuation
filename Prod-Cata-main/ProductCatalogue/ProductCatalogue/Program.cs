using ProductCatalogue.Menues;
using Resources.Services;


var fileService = new FileService((Path.Combine(Directory.GetCurrentDirectory(), "currentProductList.json")));
var productService = new ProductService(fileService);
var productMenu = new ProductMenu(productService);
var mainMenu = new MainMenu(productMenu);

while (true)
{
    //While loop to start the program and send the user into the startmenu.
    mainMenu.StartMenu();

    //Asking the user to press enter to continue after they come out of the menu before the program loops.
    Console.Write("> Press enter to continue. < ");
    Console.ReadKey();
}
