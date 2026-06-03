using CompraProgramada.Domain.CustodyContext.Services;

namespace tests.CompraProgramada.Domain.Tests.CustodyContext.Services;

public class DistributionServiceTest
{
    [Fact]
    public void CalculateDistribution_WithOneClient_ShouldReceiveAllAvailableStocks()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var clients = new List<(Guid Id, int MonthlyValue)> { (clientId, 1000) };

        // Act
        var result = CalculateDistributionService.CalculateDistribution(clients, 30, 1000m);

        // Assert
        result.Single(r => r.GraphicAccountId == clientId).QuantityOfStocks.Should().Be(30);
    }

    [Fact]
    public void CalculateDistribution_WithThreeClients_ShouldDistributeProportionally()
    {
        // Arrange
        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();
        var idC = Guid.NewGuid();
        var clients = new List<(Guid Id, int MonthlyValue)>
        {
            (idA, 1000),
            (idB, 2000),
            (idC,  500)
        };

        // Act
        var result = CalculateDistributionService.CalculateDistribution(clients, 30, 3500m);

        // Assert
        result.Single(r => r.GraphicAccountId == idA).QuantityOfStocks.Should().Be(8);
        result.Single(r => r.GraphicAccountId == idB).QuantityOfStocks.Should().Be(17);
        result.Single(r => r.GraphicAccountId == idC).QuantityOfStocks.Should().Be(4);
    }

    [Fact]
    public void CalculateDistribution_WithThreeClients_TotalDistributedShouldBeLessThanOrEqualToAvailable()
    {
        // Arrange
        var clients = new List<(Guid Id, int MonthlyValue)>
        {
            (Guid.NewGuid(), 1000),
            (Guid.NewGuid(), 2000),
            (Guid.NewGuid(),  500)
        };

        // Act
        var result = CalculateDistributionService.CalculateDistribution(clients, 30, 3500m);

        // Assert
        result.Sum(r => r.QuantityOfStocks).Should().BeGreaterThan(0);
        result.Sum(r => r.QuantityOfStocks).Should().BeLessThanOrEqualTo(30);
    }

    [Fact]
    public void CalculateDistribution_WithEqualClients_ShouldDistributeEqually()
    {
        // Arrange 
        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();
        var clients = new List<(Guid Id, int MonthlyValue)>
        {
            (idA, 1000),
            (idB, 1000)
        };

        // Act
        var result = CalculateDistributionService.CalculateDistribution(clients, 10, 2000m);

        // Assert
        result.Single(r => r.GraphicAccountId == idA).QuantityOfStocks.Should().Be(5);
        result.Single(r => r.GraphicAccountId == idB).QuantityOfStocks.Should().Be(5);
    }

    [Fact]
    public void CalculateDistribution_WithResidualStocks_ShouldTruncateAndLeaveResidue()
    {
        // Arrange
        var clients = new List<(Guid Id, int MonthlyValue)>
        {
            (Guid.NewGuid(), 1000),
            (Guid.NewGuid(), 1000)
        };

        // Act
        var result = CalculateDistributionService.CalculateDistribution(clients, 9, 2000m);

        // Assert
        result.Sum(r => r.QuantityOfStocks).Should().Be(8);
    }
}
