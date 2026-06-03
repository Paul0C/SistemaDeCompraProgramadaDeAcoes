using CompraProgramada.Domain.PurchaseContext.Entities;
using CompraProgramada.Domain.PurchaseContext.Enums;
using CompraProgramada.Domain.PurchaseContext.Models;
using CompraProgramada.Domain.SharedContext;

namespace tests.CompraProgramada.Domain.Tests.PurchaseContext.Entities;

public class BuyOrderTest
{
    private static readonly Guid ValidMasterAccountId = Guid.NewGuid();

    private static BuyOrder CreateValidBuyOrder(
        Guid? masterAccountId = null,
        string ticker = "PETR4",
        int quantity = 10,
        decimal unitPrice = 35m) =>
        BuyOrder.Create(
            masterAccountId ?? ValidMasterAccountId,
            new StockToBuy(ticker, quantity, unitPrice));

    [Fact]
    public void Create_WithValidData_ShouldReturnBuyOrderWithCorrectProperties()
    {
        // Arrange
        var masterAccountId = Guid.NewGuid();
        var stock = new StockToBuy("VALE3", 14, 62m);

        // Act
        var order = BuyOrder.Create(masterAccountId, stock);

        // Assert
        order.MasterAccountId.Should().Be(masterAccountId);
        order.Ticker.Should().Be("VALE3");
        order.Quantity.Should().Be(14);
        order.UnitPrice.Should().Be(62m);
    }

    [Fact]
    public void Create_WithValidData_ShouldGenerateId()
    {
        // Act
        var order = CreateValidBuyOrder();

        // Assert
        order.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Create_WithValidData_ShouldSetExecutionDate()
    {
        // Arrange
        var before = DateTime.UtcNow;

        // Act
        var order = CreateValidBuyOrder();

        // Assert
        order.ExecutionDate.Should().BeOnOrAfter(before).And.BeOnOrBefore(DateTime.UtcNow);
    }

    [Fact]
    public void Create_WithTickerNotEndingInF_ShouldSetMarketTypeToLot()
    {
        // Act
        var order = CreateValidBuyOrder(ticker: "PETR4");

        // Assert
        order.MarketType.Should().Be(MarketType.Lot);
    }

    [Fact]
    public void Create_WithTickerEndingInF_ShouldSetMarketTypeToFractional()
    {
        // Act
        var order = CreateValidBuyOrder(ticker: "PETR4F");

        // Assert
        order.MarketType.Should().Be(MarketType.Fractional);
    }

    [Fact]
    public void Create_WithEmptyMasterAccountId_ShouldThrowDomainException()
    {
        // Act
        Action act = () => CreateValidBuyOrder(masterAccountId: Guid.Empty);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Master account id is required.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithNullOrEmptyTicker_ShouldThrowDomainException(string? ticker)
    {
        // Act
        Action act = () => BuyOrder.Create(ValidMasterAccountId, new StockToBuy(ticker!, 10, 35m));

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Ticker is required.");
    }

    [Fact]
    public void Create_WithZeroQuantity_ShouldThrowDomainException()
    {
        // Act
        Action act = () => CreateValidBuyOrder(quantity: 0);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The quantity must be greater than zero.");
    }

    [Fact]
    public void Create_WithNegativeQuantity_ShouldThrowDomainException()
    {
        // Act
        Action act = () => CreateValidBuyOrder(quantity: -1);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The quantity must be greater than zero.");
    }

    [Fact]
    public void Create_WithZeroUnitPrice_ShouldThrowDomainException()
    {
        // Act
        Action act = () => CreateValidBuyOrder(unitPrice: 0m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The unit price must be greater than zero.");
    }

    [Fact]
    public void Create_WithNegativeUnitPrice_ShouldThrowDomainException()
    {
        // Act
        Action act = () => CreateValidBuyOrder(unitPrice: -1m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The unit price must be greater than zero.");
    }
}
