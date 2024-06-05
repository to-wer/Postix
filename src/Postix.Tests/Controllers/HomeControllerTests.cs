using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Postix.Controllers;

namespace Postix.Tests.Controllers;

public class HomeControllerTests
{
    [Fact]
    public void Test_Index_ViewResult()
    {
        // Arrange
        var homeController = new HomeController(new Mock<ILogger<HomeController>>().Object);

        // Act
        var result = homeController.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Test_Privacy_ViewResult()
    {
        // Arrange
        var homeController = new HomeController(new Mock<ILogger<HomeController>>().Object);

        // Act
        var result = homeController.Privacy();

        // Assert
        Assert.IsType<ViewResult>(result);
    }
}