using Moq;
using Resources.Interface;
using Resources.Models;
using Resources.Services;

namespace Resources.Tests.Tests;

public class ProductServiceTests
{
    [Fact]
    public void AddToList_ShouldReturnTrueResponse_IfProductGotAdded_AndListContainsProduct()
    {
        //Arrange;  (Where you line all the things up that you need to be able to test the method.)
            //Creating the mock fileservice so that the test doesn't utilize the actual fileservice to send anything.
        var mockFileService = new Mock<IFileService>();
        mockFileService.Setup(fs => fs.SaveToFile(It.IsAny<string>()))
            .Returns(new Response<string> { Succeeded = true });
        var productService = new ProductService(mockFileService.Object);

            //Creating the product that will be used with valid parameters.
        string productId = Guid.NewGuid().ToString();
        var product = new Product(productId, "TestingProduct", 5m);

        //Act; (Where you use the method with the data that was established above.)
            //Sending the product to the addtolist method in productservice and storing the return in result.
        var result = productService.AddToList(product);

        //Assert; (Assert things that you want ot be true for the test to show green.)
            //Asserting that succeeded is true.
        Assert.True(result.Succeeded);
            //Asserting that the result.message is the same as the one given.
        Assert.Equal($"{product.ProductName} was added.", result.Message);
            //Asserting that the list in result.content contains a product with the same name, price and guid.
        Assert.Contains(result.Content, p => p.ProductId == productId && p.ProductName == "TestingProduct" && p.ProductPrice == 5m);
            //Asserting that the list in result.content contains atleast one object.
        Assert.Single(result.Content);
    }
    // [Theory]





}
