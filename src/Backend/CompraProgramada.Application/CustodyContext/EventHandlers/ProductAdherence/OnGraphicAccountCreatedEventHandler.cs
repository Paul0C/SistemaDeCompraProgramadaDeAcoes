using CompraProgramada.Domain.ClientContext.Events;
using CompraProgramada.Domain.CustodyContext.Abstractions.Repositories;
using CompraProgramada.Domain.CustodyContext.Entities;
using CompraProgramada.Domain.RecommendationBasketContext.Abstractions.Querys;
using MediatR;

namespace CompraProgramada.Application.CustodyContext.EventHandlers.ProductAdherence;

public class OnGraphicAccountCreatedEventHandler(ICustodyRepository custodyRepository, IBasketQuery basketQuery) : INotificationHandler<OnGraphicAccountCreatedEvent>
{
    public async Task Handle(OnGraphicAccountCreatedEvent notification, CancellationToken cancellationToken)
    {
        var tickerOfBasket = await basketQuery.GetCurrentTickersOfBasket();
        foreach (var ticker in tickerOfBasket)
        {
            var custodyLot = Custody.Create(notification.GraphicAccountId, ticker);
            await custodyRepository.AddAsync(custodyLot);
        }
    }
}