using CompraProgramada.Domain.CustodyContext.Entities;
using CompraProgramada.Domain.SharedContext;
using FluentAssertions;

namespace tests.CompraProgramada.Domain.Tests.CustodyContext.Entities;

public class CustodyTest
{
    private static readonly Guid ValidGraphicAccountId = Guid.NewGuid();

    private static Custody CreateValidCustody(
        Guid? graphicAccountId = null,
        string ticker = "PETR4") =>
        Custody.Create(graphicAccountId ?? ValidGraphicAccountId, ticker);

    [Fact]
    public void Create_WithValidData_ShouldReturnCustodyWithCorrectProperties()
    {
        // Arrange
        var graphicAccountId = Guid.NewGuid();
        var ticker = "VALE3";

        // Act
        var custody = Custody.Create(graphicAccountId, ticker);

        // Assert
        custody.GraphicAccountId.Should().Be(graphicAccountId);
        custody.Ticker.Should().Be(ticker);
    }

    [Fact]
    public void Create_WithValidData_ShouldGenerateId()
    {
        // Act
        var custody = CreateValidCustody();

        // Assert
        custody.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Create_WithValidData_ShouldInitializeQuantityAsZero()
    {
        // Act
        var custody = CreateValidCustody();

        // Assert
        custody.Quantity.Should().Be(0);
    }

    [Fact]
    public void Create_WithValidData_ShouldInitializeAveragePriceAsZero()
    {
        // Act
        var custody = CreateValidCustody();

        // Assert
        custody.AveragePrice.Should().Be(0);
    }

    [Fact]
    public void Create_WithEmptyGraphicAccountId_ShouldThrowDomainException()
    {
        // Act
        Action act = () => Custody.Create(Guid.Empty, "PETR4");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Invalid graphic account id.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithNullOrEmptyTicker_ShouldThrowDomainException(string? ticker)
    {
        // Act
        Action act = () => Custody.Create(ValidGraphicAccountId, ticker!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Ticker is required.");
    }


    [Fact]
    public void AddQuantity_WithFirstPurchase_ShouldSetAveragePriceEqualToUnitPrice()
    {
        // Arrange
        var custody = CreateValidCustody();

        // Act
        custody.AddQuantity(8, 35m, Guid.NewGuid());

        // Assert
        custody.AveragePrice.Should().Be(35m);
    }

    [Fact]
    public void AddQuantity_WithSecondPurchaseAtDifferentPrice_ShouldRecalculateAveragePrice()
    {
        // Arrange 
        var custody = CreateValidCustody();
        custody.AddQuantity(8, 35m, Guid.NewGuid());

        // Act
        custody.AddQuantity(10, 37m, Guid.NewGuid());

        // Assert
        var expected = (8m * 35m + 10m * 37m) / 18m;
        custody.AveragePrice.Should().BeApproximately(expected, 0.01m);
    }

    [Fact]
    public void AddQuantity_WithValidData_ShouldAccumulateQuantity()
    {
        // Arrange
        var custody = CreateValidCustody();
        custody.AddQuantity(8, 35m, Guid.NewGuid());

        // Act
        custody.AddQuantity(10, 37m, Guid.NewGuid());

        // Assert
        custody.Quantity.Should().Be(18m);
    }

    [Fact]
    public void AddQuantity_WithValidData_ShouldCreateDistributionRecord()
    {
        // Arrange
        var custody = CreateValidCustody();

        // Act
        custody.AddQuantity(8, 35m, Guid.NewGuid());

        // Assert
        custody.Distributions.Should().HaveCount(1);
    }

    [Fact]
    public void AddQuantity_CalledMultipleTimes_ShouldCreateOneDistributionPerCall()
    {
        // Arrange
        var custody = CreateValidCustody();

        // Act
        custody.AddQuantity(8, 35m, Guid.NewGuid());
        custody.AddQuantity(10, 37m, Guid.NewGuid());

        // Assert
        custody.Distributions.Should().HaveCount(2);
    }

    [Fact]
    public void AddQuantity_WithValidData_ShouldLinkDistributionToCustodyId()
    {
        // Arrange
        var custody = CreateValidCustody();

        // Act
        custody.AddQuantity(8, 35m, Guid.NewGuid());

        // Assert
        custody.Distributions.Single().BabyCustodyId.Should().Be(custody.Id);
    }

    [Fact]
    public void AddQuantity_WithValidData_ShouldUpdateLastUpdateDate()
    {
        // Arrange
        var custody = CreateValidCustody();
        var before = DateTime.UtcNow;

        // Act
        custody.AddQuantity(8, 35m, Guid.NewGuid());

        // Assert
        custody.LastUpdateDate.Should().BeOnOrAfter(before);
    }

    [Fact]
    public void AddQuantity_WithZeroQuantity_ShouldThrowDomainException()
    {
        // Arrange
        var custody = CreateValidCustody();

        // Act
        Action act = () => custody.AddQuantity(0, 35m, Guid.NewGuid());

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The quantity of stocks must be greater than zero.");
    }

    [Fact]
    public void AddQuantity_WithNegativeQuantity_ShouldThrowDomainException()
    {
        // Arrange
        var custody = CreateValidCustody();

        // Act
        Action act = () => custody.AddQuantity(-5, 35m, Guid.NewGuid());

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The quantity of stocks must be greater than zero.");
    }

    [Fact]
    public void AddQuantity_WithZeroUnitPrice_ShouldThrowDomainException()
    {
        // Arrange
        var custody = CreateValidCustody();

        // Act
        Action act = () => custody.AddQuantity(8, 0m, Guid.NewGuid());

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The unit price of stocks must be greater than zero.");
    }

    [Fact]
    public void AddQuantity_WithNegativeUnitPrice_ShouldThrowDomainException()
    {
        // Arrange
        var custody = CreateValidCustody();

        // Act
        Action act = () => custody.AddQuantity(8, -1m, Guid.NewGuid());

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The unit price of stocks must be greater than zero.");
    }

    [Fact]
    public void AddQuantity_WithEmptyBuyOrderId_ShouldThrowDomainException()
    {
        // Arrange
        var custody = CreateValidCustody();

        // Act
        Action act = () => custody.AddQuantity(8, 35m, Guid.Empty);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The buy order id is required.");
    }
}
