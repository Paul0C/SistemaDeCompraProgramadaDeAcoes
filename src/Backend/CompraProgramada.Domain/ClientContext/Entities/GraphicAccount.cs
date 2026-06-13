using System.Runtime.InteropServices;
using CompraProgramada.Domain.ClientContext.Enums;
using CompraProgramada.Domain.ClientContext.Events;
using CompraProgramada.Domain.ClientContext.ValueObjects;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.ClientContext.Entities;

public class GraphicAccount : Entity
{
    public Guid ClientId { get;}
    public AccountNumber AccountNumber { get; private set; }
    public AccountType Type { get;}
    public DateTime CreateDate { get; private set; }

    private GraphicAccount(Guid clientId) : base()
    {
        AccountNumber = new AccountNumber();
        Type = AccountType.Baby;
        CreateDate = DateTime.Now;
        ClientId = clientId;
    }

    public static GraphicAccount Create(Guid clientId)
    {
        var graphicAccount = new GraphicAccount(clientId);
        graphicAccount.AddDomainEvent(new OnGraphicAccountCreatedEvent(graphicAccount.Id));
        return graphicAccount;
    }
}