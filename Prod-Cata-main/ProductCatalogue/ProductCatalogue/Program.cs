using ProductCatalogue.Menues;
using Resources.Services;


var fileService = new FileService((Path.Combine(Directory.GetCurrentDirectory(), "currentProductList.json")));
var productService = new ProductService(fileService);
var productMenu = new ProductMenu(productService);
var mainMenu = new MainMenu(productMenu);

while (true)
{
    mainMenu.StartMenu();
    Console.Write("> Press enter to continue. < ");
    Console.ReadKey();
}
