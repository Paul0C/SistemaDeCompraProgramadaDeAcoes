using CompraProgramada.Domain.ClientContext.Entities;
using CompraProgramada.Domain.CustodyContext.Entities;
using CompraProgramada.Domain.PurchaseContext.Entities;
using CompraProgramada.Domain.RecommendationBasketContext.Entities;
using CompraProgramada.Domain.SharedContext;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace CompraProgramada.Infrastructure.Persistence.SharedContext.Context;

public class AppDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public DbSet<Client> Clients { get; set; }
    public DbSet<RecommendationBasket> Baskets { get; set; }
    public DbSet<BuyOrder> BuyOrders { get; set; }
    public DbSet<Custody> Custodys { get; set; }

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }

    public async Task<int> CommitAsync(CancellationToken ct = default)
    {
        await PublishDomainEvents(ct);
        return await base.SaveChangesAsync(ct);
    }

    private async Task PublishDomainEvents(CancellationToken ct)
    {
        var aggregates = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var events = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        foreach (var aggregate in aggregates)
            aggregate.ClearDomainEvents();

        foreach (var evt in events)
            await _mediator.Publish(evt, ct);
    }
}
