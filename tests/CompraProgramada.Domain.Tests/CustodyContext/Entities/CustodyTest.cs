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
}
  
