using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.ClientContext.Events;

public sealed record OnGraphicAccountCreatedEvent(Guid GraphicAccountId) : IDomainEvent;