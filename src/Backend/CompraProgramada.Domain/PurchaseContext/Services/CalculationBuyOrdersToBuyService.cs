using CompraProgramada.Domain.PurchaseContext.Models;

namespace CompraProgramada.Domain.PurchaseContext.Services;

public sealed class CalculationBuyOrdersToBuyService
{
    public static List<StockToBuy> CalculateStocksToBuy(
        List<decimal> valueOfClients, 
        List<ActuallyTickerOfRecommendationBasket> actuallyBasket, 
        List<ClosingPrice> closingPrices,
        List<MasterPreviousBalance> masterPreviousBalance)
    {
        var consolidatedValueOfClients = valueOfClients.Sum(c => c / 3);
        
        var stocksToBuyResult = new List<StockToBuy>();
        foreach (var itemBasket in actuallyBasket)
        {
            var valueToBuy = consolidatedValueOfClients * itemBasket.Percentage;
            
            var valueOfStock = closingPrices.FirstOrDefault(cp => cp.Ticker == itemBasket.Ticker).Price;

            var quantity = Math.Floor(valueToBuy / valueOfStock);
            var quantityPreviousMaster = masterPreviousBalance.FirstOrDefault(cp => cp.Ticker == itemBasket.Ticker).Quantity;
            var quantityStocksToBuy = (int)quantity - quantityPreviousMaster;

            var stocksToBuy = new List<StockToBuy>();
            var quantityStocksToBuyFractional = quantityStocksToBuy % 100;
            var quantityStocksToBuyLot = quantityStocksToBuy -  quantityStocksToBuyFractional;

            if (quantityStocksToBuyFractional > 0)
                stocksToBuy.Add(new StockToBuy(itemBasket.Ticker + "F", quantityStocksToBuyFractional, valueOfStock));
            
            if (quantityStocksToBuyLot > 0)
                stocksToBuy.Add(new StockToBuy(itemBasket.Ticker, quantityStocksToBuyLot, valueOfStock));
            
            stocksToBuyResult.AddRange(stocksToBuy);
        }

        return stocksToBuyResult;
    }
}
