using Moq;
using Resources.Interface;
using Resources.Models;
using Resources.Services;

namespace Resources.Tests.Tests;

public class ProductServiceTests
{
    [Fact]
    public void AddToList_ShouldReturnTrueResponse()
    {
        //Arrange
        var mockFileService = new Mock<IFileService>();

        mockFileService.Setup(fs => fs.SaveToFile(It.IsAny<string>()))
            .Returns(new Response<string> { Succeeded = true });

        var productService = new ProductService(mockFileService.Object);

        string productId = Guid.NewGuid().ToString();
        var product = new Product(productId, "Tomat", 5m);

        //Act;
        var result = productService.AddToList(product);

        //Assert;
        Assert.True(result.Succeeded);
        Assert.Equal($"{product.ProductName} was added.", result.Message);

        mockFileService.Verify(fs => fs.SaveToFile(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void GetAllProducts_ToCheckIfProduct_WasAddedToList()
    {
        //Arrange
        var mockFileService = new Mock<IFileService>();

        mockFileService.Setup(fs => fs.GetFromFile())
            .Returns(new Response<string>
            {
                Succeeded = true,
                Content = "[{\"ProductId\":\"P123\",\"ProductName\":\"TestProduct\",\"ProductPrice\":100.0}]"
            });
        var productService = new ProductService(mockFileService.Object);

        //Act
        var allProductsResponse = productService.GetAllProducts();

        //Assert
        Assert.True(allProductsResponse.Succeeded);
        Assert.Contains(allProductsResponse.Content, p => p.ProductId == "P123" && p.ProductName == "TestProduct");
    }
}
