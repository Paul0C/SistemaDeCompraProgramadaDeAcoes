using CompraProgramada.Application.SharedContext.Results;
using CompraProgramada.Application.SharedContext.UseCases.Abstractions;
using CompraProgramada.Domain.ClientContext.Abstractions.Queries;
using CompraProgramada.Domain.CustodyContext.Abstractions.Queries;
using CompraProgramada.Domain.PurchaseContext.Abstractions.Repositories;
using CompraProgramada.Domain.PurchaseContext.Entities;
using CompraProgramada.Domain.PurchaseContext.Events;
using CompraProgramada.Domain.PurchaseContext.Models;
using CompraProgramada.Domain.PurchaseContext.Services;
using CompraProgramada.Domain.QuoteContext.Abstraction.Services;
using CompraProgramada.Domain.RecommendationBasketContext.Abstractions.Querys;
using CompraProgramada.Domain.SharedContext;
using MediatR;

namespace CompraProgramada.Application.PurchaseContext.Commands.ExecutePurchase;

public class ExecutePurchaseCommandHandler(IClientQuery clientQuery, IBasketQuery basketQuery, 
    ICotahistService cotahistService, ICustodyQuery custodyQuery,
    IPurchaseRepository purchaseRepository, IMediator mediator, IUnitOfWork unitOfWork) : ICommandHandler<ExecutePurchaseCommand>
{
    public async Task<Result> Handle(ExecutePurchaseCommand request, CancellationToken cancellationToken)
    {
        var clients = await clientQuery.GetClientActiveToPurchaseAsync();
        var currentBasket = await basketQuery.GetActiveBasket();
        var closingPricesOfCurrentBasket = await cotahistService.GetClosingPrices(request.ReferenceDate, currentBasket.Select(s => s.Ticker).ToList());
        var masterPreviousBalance = await custodyQuery.GetMasterPreviousBalance();
        var masterCustodyId = await custodyQuery.GetMasterCustodyId();

        var stocksToBuy = CalculationBuyOrdersToBuyService.CalculateStocksToBuy(clients.Select(c => c.MonthlyValue).ToList(), ToActuallyTickerOfRecommendationBasket(currentBasket),
            ToClosingPrice(closingPricesOfCurrentBasket), ToMasterPreviousBalance(masterPreviousBalance));

        var listOfOrdersPurchased = new List<PurchasedOrder>();
        foreach (var stockToBuy in stocksToBuy)
        {
            var buyOrder = BuyOrder.Create(masterCustodyId, stockToBuy);
            await purchaseRepository.AddBuyOrderAsync(buyOrder);
            listOfOrdersPurchased.Add(new PurchasedOrder(buyOrder.Id, buyOrder.Ticker, buyOrder.Quantity, buyOrder.UnitPrice, buyOrder.MarketType.ToString()));
        }

        await mediator.Publish(new PurchaseOfStocksEvent(listOfOrdersPurchased), cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        
        var response = await GenerateResponse(request.ReferenceDate, listOfOrdersPurchased, clients);
        return Result.Success(response);
    }

    private async Task<ExecutePurchaseCommandResponse> GenerateResponse(DateTime executionDate, List<PurchasedOrder> ordersPurchased, List<ClientDto> clients)
    {
        var purchaseOrders = ordersPurchased
            .GroupBy(x => x.Ticker.Replace("F", ""))
            .Select(group =>
            {
                return new PurchaseOrderResponse(
                    Ticker: group.Key,
                    TotalQuantity: group.Sum(x => x.Quantity),
                    Details: group
                        .Select(x => new PurchaseOrderDetailResponse(
                            Type: x.Type,
                            Ticker: x.Ticker,
                            Quantity: x.Quantity))
                        .ToList(),
                    UnitPrice: group.First().UnitPrice,
                    TotalValue: group.Sum(x => x.Quantity * x.UnitPrice)
                );
            })
            .ToList()
            .AsReadOnly();
        
        var assetsPerClient = await custodyQuery.GetTickersBoughtOfClientAsync(ordersPurchased.Select(o => o.OrderBuyId).ToList());
        
        var distributions = new List<ClientAllocationResponse>();
        foreach (var client in clients)
        {
            var assets = assetsPerClient.Where(a => a.GraphicAccountId == client.GraphicAccountId).Select(a => new ClientAssetAllocationResponse(a.Ticker, a.Quantity)).ToList().AsReadOnly();
            distributions.Add(new ClientAllocationResponse(client.Id, client.Name, client.MonthlyValue, assets));
        }

        var masterPreviousBalance = await custodyQuery.GetMasterPreviousBalance();

        var masterCustodyResidualResponse =
            masterPreviousBalance.Select(x => new MasterCustodyResidualResponse(x.Ticker, x.Quantity)).ToList().AsReadOnly();
        
        return new ExecutePurchaseCommandResponse(executionDate, clients.Count, clients.Sum(c => c.MonthlyValue / 3),
            purchaseOrders, distributions.AsReadOnly(), masterCustodyResidualResponse, 0, $"Programmed Purchase execute successfully to {clients.Count} clients.");
    }
    
    private static List<ActuallyTickerOfRecommendationBasket> ToActuallyTickerOfRecommendationBasket(
        List<ActiveBasketDto> activeBasketDtos)
        => activeBasketDtos.Select(x => new ActuallyTickerOfRecommendationBasket(x.Ticker, x.Percentage)).ToList();
    
    private static List<ClosingPrice> ToClosingPrice(
        List<ClosingPriceDto> activeBasketDtos)
        => activeBasketDtos.Select(x => new ClosingPrice(x.Ticker, x.Price)).ToList();

    private static List<MasterPreviousBalance> ToMasterPreviousBalance(
        List<MasterPreviousBalanceDto> masterPreviousBalanceDtos)
        => masterPreviousBalanceDtos.Select(x => new MasterPreviousBalance(x.Ticker, x.Quantity)).ToList();
}