using BTCSpy.Presentation.Controllers;
using Contracts;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Tests;

public class BTCSpyControllerTests
{
    private readonly ILoggerManager _logger;
    private readonly IServiceManager _service;
    private readonly BTCSpyController _controller;

    // Arrange:
    private BestPriceOrdersQueryParametersDto TypicalQueryParameters { get; init; } = new()
    {
        Type = "Buy",
        BtcAmount = 5
    };

    public BTCSpyControllerTests()
    {
        _logger = new LoggerManager();
        _service = new ServiceManager(_logger);
        _controller = new BTCSpyController(_service);
    }

    [Fact]
    public void Get_ValidParamsPassed_ReturnsOkResult()
    {
        // Act
        var okResult = _controller.GetBestPriceOrders(TypicalQueryParameters);

        // Assert
        Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
    }

    [Fact]
    public void Get_ValidParamsPassed_ReturnsAllItems()
    {
        // Act
        var okResult = _controller.GetBestPriceOrders(TypicalQueryParameters) as OkObjectResult;

        // Assert
        var items = Assert.IsType<List<BestPriceOrderDto>>(okResult.Value);
        Assert.Equal(11, items.Count);
    }

    [Fact]
    public void Get_ValidParamsPassed_ReturnsRightItem()
    {
        // Act
        var okResult = _controller.GetBestPriceOrders(TypicalQueryParameters) as OkObjectResult;

        // Assert
        var items = Assert.IsType<List<BestPriceOrderDto>>(okResult.Value);
        BestPriceOrderDto expectedLastItem = new()
        {
            OrderBookId = 1,
            OrderId = null,
            Amount = 4.24m,
            Price = 2902
        };
        Assert.Equal(expectedLastItem, items.Last());
    }

    [Fact]
    public void Get_InvalidParamsPassed_ReturnsBadRequest()
    {
        // Arrange
        BestPriceOrdersQueryParametersDto invalidQueryParams = new()
        {
            Type = "Something invalid here...",     // invalid param
            BtcAmount = 5
        };

        // Act
        var badResponse = _controller.GetBestPriceOrders(invalidQueryParams);

        // Assert
        Assert.IsType<BadRequestObjectResult>(badResponse);
    }

}
