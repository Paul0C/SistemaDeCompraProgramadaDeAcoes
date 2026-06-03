using System.Collections.Immutable;
using CompraProgramada.Domain.CustodyContext.Entities;

namespace CompraProgramada.Domain.CustodyContext.Services;

public class CalculateDistributionService
{
    public static List<(Guid GraphicAccountId, int QuantityOfStocks)> CalculateDistribution(List<(Guid Id, int MonthlyValue)> clients, int totalStocksAvailable, decimal totalValueOfClients)
    {
        var distributionsReturn = new List<(Guid GraphicAccountId, int quantityOfStocks)>();
        foreach (var client in clients)
        {
            var proportionOfClient = client.MonthlyValue / totalValueOfClients;
          
            var quantityOfStockToClient = Math.Floor(proportionOfClient * totalStocksAvailable);
            distributionsReturn.Add((client.Id,  (int)quantityOfStockToClient));
        }

        return distributionsReturn;
    }
}