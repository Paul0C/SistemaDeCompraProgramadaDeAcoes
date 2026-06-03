using CompraProgramada.Domain.CustodyContext.Entities;
using CompraProgramada.Domain.SharedContext;
using FluentAssertions;

namespace tests.CompraProgramada.Domain.Tests.CustodyContext.Entities;

public class DistributionTest
{
    private static readonly Guid ValidBuyOrderId = Guid.NewGuid();
    private static readonly Guid ValidBabyCustodyId = Guid.NewGuid();

    private static Distribution CreateValidDistribution(
        Guid? buyOrderId = null,
        Guid? babyCustodyId = null,
        string ticker = "PETR4",
        int quantity = 10,
        decimal unitPrice = 35m) =>
        Distribution.Create(
            buyOrderId ?? ValidBuyOrderId,
            babyCustodyId ?? ValidBabyCustodyId,
            ticker,
            quantity,
            unitPrice,
            DateTime.UtcNow);

    [Fact]
    public void Create_WithValidData_ShouldReturnDistributionWithCorrectProperties()
    {
        // Arrange
        var buyOrderId = Guid.NewGuid();
        var babyCustodyId = Guid.NewGuid();
        var ticker = "VALE3";
        var quantity = 8;
        var unitPrice = 62m;
        var date = DateTime.UtcNow;

        // Act
        var distribution = Distribution.Create(buyOrderId, babyCustodyId, ticker, quantity, unitPrice, date);

        // Assert
        distribution.BuyOrderId.Should().Be(buyOrderId);
        distribution.BabyCustodyId.Should().Be(babyCustodyId);
        distribution.Ticker.Should().Be(ticker);
        distribution.Quantity.Should().Be(quantity);
        distribution.UnitPrice.Should().Be(unitPrice);
        distribution.DistributionDate.Should().Be(date);
    }

    [Fact]
    public void Create_WithValidData_ShouldGenerateId()
    {
        // Act
        var distribution = CreateValidDistribution();

        // Assert
        distribution.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Create_WithEmptyBuyOrderId_ShouldThrowDomainException()
    {
        // Act
        Action act = () => CreateValidDistribution(buyOrderId: Guid.Empty);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The order buy id is required.");
    }

    [Fact]
    public void Create_WithEmptyBabyCustodyId_ShouldThrowDomainException()
    {
        // Act
        Action act = () => CreateValidDistribution(babyCustodyId: Guid.Empty);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The custody id is required.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithNullOrEmptyTicker_ShouldThrowDomainException(string? ticker)
    {
        // Act
        Action act = () => CreateValidDistribution(ticker: ticker!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The ticker is required.");
    }

    [Fact]
    public void Create_WithZeroQuantity_ShouldThrowDomainException()
    {
        // Act
        Action act = () => CreateValidDistribution(quantity: 0);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The quantity of stocks must be greater than zero.");
    }

    [Fact]
    public void Create_WithNegativeQuantity_ShouldThrowDomainException()
    {
        // Act
        Action act = () => CreateValidDistribution(quantity: -1);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The quantity of stocks must be greater than zero.");
    }

    [Fact]
    public void Create_WithZeroUnitPrice_ShouldThrowDomainException()
    {
        // Act
        Action act = () => CreateValidDistribution(unitPrice: 0m);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Create_WithNegativeUnitPrice_ShouldThrowDomainException()
    {
        // Act
        Action act = () => CreateValidDistribution(unitPrice: -1m);

        // Assert
        act.Should().Throw<DomainException>();
    }
}
