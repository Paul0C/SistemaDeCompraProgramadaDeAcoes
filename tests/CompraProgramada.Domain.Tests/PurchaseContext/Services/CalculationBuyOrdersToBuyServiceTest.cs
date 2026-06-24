using CompraProgramada.Domain.PurchaseContext.Models;
using CompraProgramada.Domain.PurchaseContext.Services;

namespace tests.CompraProgramada.Domain.Tests.PurchaseContext.Services;

public class CalculationBuyOrdersToBuyServiceTest
{
    private static List<ActuallyTickerOfRecommendationBasket> StandardBasket() =>
    [
        new ("PETR4", 0.3m),
        new ("VALE3", 0.25m),
        new ("ITUB4", 0.2m),
        new ("BBDC4", 0.15m),
        new ("WEGE3", 0.1m)
    ];

    private static List<ClosingPrice> StandardPrices() =>
    [
        new ("PETR4", 35m),
        new ("VALE3", 62m),
        new ("ITUB4", 30m),
        new ("BBDC4", 15m),
        new ("WEGE3", 40m)
    ];

    private static List<MasterPreviousBalance> EmptyMasterBalance() =>
    [
        new ("PETR4", 0),
        new ("VALE3", 0),
        new ("ITUB4", 0),
        new ("BBDC4", 0),
        new ("WEGE3", 0)
    ];

    [Fact]
    public void CalculateStocksToBuy_WithThreeClients_ShouldConsolidateOneThirdEach()
    {
        // Arrange 
        var clients = new List<decimal> { 3000m, 6000m, 1500m };

        // Act
        var result = CalculationBuyOrdersToBuyService.CalculateStocksToBuy(
            clients, StandardBasket(), StandardPrices(), EmptyMasterBalance());

        // Assert
        var petr4Fractional = result.SingleOrDefault(s => s.Ticker == "PETR4F");
        petr4Fractional.Should().NotBeNull();
        petr4Fractional.Quantity.Should().Be(30);
    }
    

    [Fact]
    public void CalculateStocksToBuy_WithQuantityLessThan100_ShouldOnlyProduceFractionalOrder()
    {
        // Arrange
        var clients = new List<decimal> { 3000m, 6000m, 1500m };

        // Act
        var result = CalculationBuyOrdersToBuyService.CalculateStocksToBuy(
            clients, StandardBasket(), StandardPrices(), EmptyMasterBalance());

        // Assert
        var zeroQuantityLots = result.Where(s => !s.Ticker.EndsWith('F') && s.Quantity == 0).ToList();
        zeroQuantityLots.Should().BeEmpty("buy orders with quantity zero should not be created");
    }

    [Fact]
    public void CalculateStocksToBuy_WithQuantityExactly100_ShouldOnlyProduceLotOrder()
    {
        // Arrange
        var clients = new List<decimal> { 35000m };
        var basket = new List<ActuallyTickerOfRecommendationBasket> { new ("PETR4", 0.3m) };
        var prices = new List<ClosingPrice> { new ("PETR4", 35m) };
        var master = new List<MasterPreviousBalance> { new ("PETR4", 0) };

        // Act
        var result = CalculationBuyOrdersToBuyService.CalculateStocksToBuy(clients, basket, prices, master);

        // Assert
        result.Should().ContainSingle(s => s.Ticker == "PETR4" && s.Quantity == 100);
        result.Should().NotContain(s => s.Ticker == "PETR4F");
    }

    [Fact]
    public void CalculateStocksToBuy_WithQuantity350_ShouldProduceLotAndFractionalOrders()
    {
        // Arrange 
        var clients = new List<decimal> { 122500m };
        var basket = new List<ActuallyTickerOfRecommendationBasket> { new ("PETR4", 0.3m) };
        var prices = new List<ClosingPrice> { new ("PETR4", 35m) };
        var master = new List<MasterPreviousBalance> { new ("PETR4", 0) };

        // Act
        var result = CalculationBuyOrdersToBuyService.CalculateStocksToBuy(clients, basket, prices, master);

        // Assert
        result.Single(s => s.Ticker == "PETR4").Quantity.Should().Be(300);
        result.Single(s => s.Ticker == "PETR4F").Quantity.Should().Be(50);
    }
    
    [Fact]
    public void CalculateStocksToBuy_WithMasterBalance_ShouldDeductFromQuantityToBuy()
    {
        // Arrange
        var clients = new List<decimal> { 3000m, 6000m, 1500m };
        var master = new List<MasterPreviousBalance>
        {
            new ("PETR4", 2),
            new ("VALE3", 0),
            new ("ITUB4", 1),
            new ("BBDC4", 0),
            new ("WEGE3", 0)
        };

        // Act
        var result = CalculationBuyOrdersToBuyService.CalculateStocksToBuy(
            clients, StandardBasket(), StandardPrices(), master);

        // Assert
        var petr4Total = result.Where(s => s.Ticker is "PETR4" or "PETR4F")
                               .Sum(s => s.Quantity);
        petr4Total.Should().Be(28);
    }

    [Fact]
    public void CalculateStocksToBuy_WhenMasterBalanceCoversEntireQuantity_ShouldNotGenerateOrder()
    {
        // Arrange 
        var clients = new List<decimal> { 1050m }; 
        var basket = new List<ActuallyTickerOfRecommendationBasket> { new ("PETR4", 30m) };
        var prices = new List<ClosingPrice> { new ("PETR4", 35m) };
        var master = new List<MasterPreviousBalance> { new ("PETR4", 100) };

        // Act
        var result = CalculationBuyOrdersToBuyService.CalculateStocksToBuy(clients, basket, prices, master);

        // Assert 
        var petr4Orders = result.Where(s => s.Ticker.StartsWith("PETR4")).ToList();
        petr4Orders.Should().OnlyContain(s => s.Quantity > 0,
            "Buy order without quantity or with negative quantity should not be created");
    }

    [Fact]
    public void CalculateStocksToBuy_WithNonDivisibleValue_ShouldTruncateQuantity()
    {
        // Arrange
        var clients = new List<decimal> { 3000m, 6000m, 1500m };

        // Act
        var result = CalculationBuyOrdersToBuyService.CalculateStocksToBuy(
            clients, StandardBasket(), StandardPrices(), EmptyMasterBalance());

        // Assert
        var vale3Total = result.Where(s => s.Ticker is "VALE3" or "VALE3F")
                               .Sum(s => s.Quantity);
        vale3Total.Should().Be(14);
    }
}
