using CompraProgramada.Domain.RecommendationBasketContext.Entities;
using CompraProgramada.Domain.RecommendationBasketContext.Events;
using CompraProgramada.Domain.SharedContext;

namespace tests.CompraProgramada.Domain.Tests.RecommendationBasketContext.Entities;

public class RecommendationBasketTest
{
    private static List<(string Ticker, decimal Percentage)> ValidItems() =>
    [
        ("PETR4", 20m),
        ("VALE3", 20m),
        ("ITUB4", 20m),
        ("BBDC4", 20m),
        ("ABEV3", 20m)
    ];

    private static RecommendationBasket CreateValidBasket(
        string name = "Top Five - Fevereiro 2026",
        List<(string Ticker, decimal Percentage)>? items = null) =>
        RecommendationBasket.Create(name, items ?? ValidItems());

    [Fact]
    public void Create_WithValidData_ShouldReturnBasketWithCorrectName()
    {
        // Arrange
        var name = "Top Five - Fevereiro 2026";

        // Act
        var basket = RecommendationBasket.Create(name, ValidItems());

        // Assert
        basket.Name.Should().Be(name);
    }

    [Fact]
    public void Create_WithValidData_ShouldSetActiveToTrue()
    {
        // Act
        var basket = CreateValidBasket();

        // Assert
        basket.Active.Should().BeTrue();
    }

    [Fact]
    public void Create_WithValidData_ShouldHaveDeactivationDateNull()
    {
        // Act
        var basket = CreateValidBasket();

        // Assert
        basket.DeactivationDate.Should().BeNull();
    }

    [Fact]
    public void Create_WithValidData_ShouldAddExactlyFiveItems()
    {
        // Act
        var basket = CreateValidBasket();

        // Assert
        basket.BasketItems.Should().HaveCount(RecommendationBasket.QuantitiesOfStockAllowed);
    }


    [Fact]
    public void Create_WithValidData_ShouldBindAllItemsToBasketId()
    {
        // Act
        var basket = CreateValidBasket();

        // Assert
        basket.BasketItems.Should().OnlyContain(item => item.RecommendationBasketId == basket.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithNullOrEmptyName_ShouldThrowDomainException(string? name)
    {
        // Act
        Action act = () => RecommendationBasket.Create(name!, ValidItems());

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The name of recommendation basket is required");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    [InlineData(6)]
    public void Create_WithWrongNumberOfItems_ShouldThrowDomainException(int count)
    {
        // Arrange
        var items = Enumerable.Range(1, count)
            .Select(i => ($"STK{i}", 100m / count))
            .ToList();

        // Act
        Action act = () => RecommendationBasket.Create("Portfolio", items);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage($"The recommendation basket need {RecommendationBasket.QuantitiesOfStockAllowed} stocks.");
    }

    [Fact]
    public void Create_WithItemsNotSummingToOneHundredPercent_ShouldThrowDomainException()
    {
        // Arrange — 5 items summing to 50%, not 100%
        var items = new List<(string Ticker, decimal Percentage)>
        {
            ("PETR4", 10m),
            ("VALE3", 10m),
            ("ITUB4", 10m),
            ("BBDC4", 10m),
            ("ABEV3", 10m)
        };

        // Act
        Action act = () => RecommendationBasket.Create("Top Five - Fevereiro 2026", items);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage($"The sum of percentage must be exactly {RecommendationBasket.SumStocksInTheBasket}%. Actual sum: {items.Sum(i => i.Percentage)}%");
    }

    [Fact]
    public void CreateWithRebalancing_WithValidData_ShouldReturnNewBasket()
    {
        // Arrange
        var oldBasket = CreateValidBasket("Top Five - Fevereiro 2026");
        var newItems = new List<(string Ticker, decimal Percentage)>
        {
            ("WEGE3", 30m),
            ("RENT3", 20m),
            ("LREN3", 20m),
            ("MGLU3", 15m),
            ("RADL3", 15m)
        };

        // Act
        var newBasket = RecommendationBasket.CreateWithRebalancing("Top Five - Marco 2026", newItems, oldBasket);

        // Assert
        newBasket.Should().NotBeNull();
        newBasket.Name.Should().Be("Top Five - Marco 2026");
        newBasket.BasketItems.Should().HaveCount(RecommendationBasket.QuantitiesOfStockAllowed);
    }

    [Fact]
    public void CreateWithRebalancing_WithValidData_ShouldRaiseRebalancingDomainEvent()
    {
        // Arrange
        var oldBasket = CreateValidBasket("Top Five - Fevereiro 2026");
        var newItems = new List<(string Ticker, decimal Percentage)>
        {
            ("WEGE3", 30m),
            ("RENT3", 20m),
            ("LREN3", 20m),
            ("MGLU3", 15m),
            ("RADL3", 15m)
        };

        // Act
        var newBasket = RecommendationBasket.CreateWithRebalancing("Top Five - Marco 2026", newItems, oldBasket);

        // Assert
        newBasket.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<RebalancingChangedBasketEvent>();
    }

    [Fact]
    public void CreateWithRebalancing_WithValidData_ShouldRaiseEventWithCorrectOldAndNewStocks()
    {
        // Arrange
        var oldBasket = CreateValidBasket("Top Five - Fevereiro 2026");
        var newItems = new List<(string Ticker, decimal Percentage)>
        {
            ("WEGE3", 30m),
            ("RENT3", 20m),
            ("LREN3", 20m),
            ("MGLU3", 15m),
            ("RADL3", 15m)
        };

        var expectedOldStocks = oldBasket.BasketItems
            .Select(bi => (bi.Ticker, bi.Percentage))
            .ToList();

        // Act
        var newBasket = RecommendationBasket.CreateWithRebalancing("Top Five - Marco 2026", newItems, oldBasket);
        var @event = (RebalancingChangedBasketEvent)newBasket.DomainEvents.Single();

        // Assert
        @event.StocksOldBasket.Should().BeEquivalentTo(expectedOldStocks);
        @event.StocksNewBasket.Should().BeEquivalentTo(newItems);
    }

    [Fact]
    public void ChangeBasket_WhenBasketIsActive_ShouldDeactivateBasket()
    {
        // Arrange
        var basket = CreateValidBasket();

        // Act
        basket.ChangeBasket();

        // Assert
        basket.Active.Should().BeFalse();
    }

    [Fact]
    public void ChangeBasket_WhenBasketIsActive_ShouldSetDeactivationDate()
    {
        // Arrange
        var basket = CreateValidBasket();
        var before = DateTime.Now;

        // Act
        basket.ChangeBasket();

        // Assert
        basket.DeactivationDate.Should().NotBeNull();
        basket.DeactivationDate.Should().BeOnOrAfter(before).And.BeOnOrBefore(DateTime.Now);
    }
}
