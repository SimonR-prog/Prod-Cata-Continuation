using Moq;
using Resources.Interface;
using Resources.Models;
using Resources.Services;

namespace Resources.Tests.Tests;

public class ProductServiceTests
{

    private Mock<IFileService> _fileServiceMock;
    private ProductService _productService;

    //Constructor for the private fields above.
    public ProductServiceTests()
    {
        _fileServiceMock = new Mock<IFileService>();
        _productService = new ProductService(_fileServiceMock.Object);
    }

    [Fact]
    public void MethodAddToList_ShouldReturnTrueResponse_IfProductGotAdded_AndListContainsProduct()
    {
        //Arrange;  (Where you line all the things up that you need to be able to test the method.)
            //Creating the mock fileservice so that the test doesn't utilize the actual fileservice to send anything.
        _fileServiceMock.Setup(fs => fs.SaveToFile(It.IsAny<string>()))
            .Returns(new Response<string> { Succeeded = true });

            //Creating the product that will be used with valid parameters.
        string productId = Guid.NewGuid().ToString();
        var testProduct = new Product(productId, "TestingProduct", 5m);

        //Act; (Where you use the method with the data that was established above.)
            //Sending the product to the addtolist method in productservice and storing the return in result.
        var result = _productService.AddToList(testProduct);

        //Assert; (Assert things that you want ot be true for the test to show green.)
            //Asserting that succeeded is true.
        Assert.True(result.Succeeded);
            //Asserting that the result.message is the same as the one given.
        Assert.Equal($"{testProduct.ProductName} was added.", result.Message);
            //Asserting that the list in result.content contains a product with the same name, price and guid.
        Assert.Contains(result.Content, p => p.ProductId == productId && p.ProductName == "TestingProduct" && p.ProductPrice == 5m);
            //Asserting that the list in result.content contains atleast one object.
        Assert.Single(result.Content);
    }


    //The invalid product check in AddToList() checks that neither the name or id
    // of the product is null or empty and that the price is not a negative number.
    [Theory]
    [InlineData("", "TestingProduct", 1)]
    [InlineData("1", "", 1)]
    [InlineData("1", "TestingProduct", -5)]
    public void MethodAddToList_ShouldReturnFalse_WhenInvalidProductsGetSent(string productId, string productName, decimal productPrice)
    {
        //Arrange;
        _fileServiceMock.Setup(fs => fs.SaveToFile(It.IsAny<string>()))
            .Returns(new Response<string>() { Succeeded = true });
        var testProduct = new Product(productId, productName, productPrice);
        
        //Act;
        var result = _productService.AddToList(testProduct);

        //Assert;
        Assert.False(result.Succeeded);
    }
    

    /*
     More tests to make;
    Add something to the list and try and add the same thing again.
    Test the invalid product method alone.
    Delete a product;
        A valid product.
        Invalid product.
    Change product;
        Add a product into the list and then try and change it.
        Can test this method also by simply sending an invalid guid to it.
     */

}
