using CompraProgramada.Domain.RecommendationBasketContext.Entities;
using CompraProgramada.Domain.SharedContext;
using FluentAssertions;

namespace tests.CompraProgramada.Domain.Tests.RecommendationBasketContext.Entities;

public class BasketItemTest
{

    private static readonly Guid ValidBasketId = Guid.NewGuid();

    private static BasketItem CreateValidBasketItem(
        Guid? recommendationBasketId = null,
        string ticker = "PETR4",
        decimal percentage = 20m) =>
        BasketItem.Create(
            recommendationBasketId ?? ValidBasketId,
            ticker,
            percentage);

    [Fact]
    public void Create_WithValidData_ShouldReturnBasketItemWithCorrectProperties()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var ticker = "VALE3";
        var percentage = 25m;

        // Act
        var item = BasketItem.Create(basketId, ticker, percentage);

        // Assert
        item.RecommendationBasketId.Should().Be(basketId);
        item.Ticker.Should().Be(ticker);
        item.Percentage.Should().Be(percentage);
    }

    [Fact]
    public void Create_WithValidData_ShouldGenerateId()
    {
        // Act
        var item = CreateValidBasketItem();

        // Assert
        item.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Create_WithEmptyRecommendationBasketId_ShouldThrowDomainException()
    {
        // Act
        Action act = () => BasketItem.Create(Guid.Empty, "PETR4", 20m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("recommendationBasketId cannot be empty");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithNullOrEmptyTicker_ShouldThrowDomainException(string ticker)
    {
        // Act
        Action act = () => BasketItem.Create(ValidBasketId, ticker, 20m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The Basket Item needs a ticker.");
    }

    [Fact]
    public void Create_WithZeroPercentage_ShouldThrowDomainException()
    {
        // Act
        Action act = () => BasketItem.Create(ValidBasketId, "PETR4", 0m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The Percentage must be greater than 0.");
    }
}
