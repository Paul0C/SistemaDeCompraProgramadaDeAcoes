using CompraProgramada.Application.SharedContext.UseCases.Abstractions;
using CompraProgramada.Domain.ClientContext.Entities;
using CompraProgramada.Domain.ClientContext.Enums;
using CompraProgramada.Domain.ClientContext.ValueObjects;

namespace CompraProgramada.Application.ClientContext.Commands.ProductAdherence;

public record ProductAdherenceCommandResponse : ICommandResponse
{
    public Guid ClientId { get; init; }
    public string Name { get; init; }
    public string Cpf { get; init; }
    public string Email { get; init; }
    public decimal MonthlyValue { get; init; }
    public bool Active { get; init; }
    public DateTime AdhesionDate { get; init; }
    public GraphicAccountResponse GraphicAccount { get; init; }

    public ProductAdherenceCommandResponse(Client client)
    {
        ClientId = client.Id;
        Name = client.Name;
        Cpf = client.CPF.Number;
        Email = client.Email.Address;
        MonthlyValue = client.MonthlyValue.Value;
        Active = client.Active;
        AdhesionDate = client.AdhesionDate;
        GraphicAccount = new GraphicAccountResponse(client.GraphicAccount.Id, client.GraphicAccount.AccountNumber.Number, client.GraphicAccount.Type.ToString(), client.GraphicAccount.CreateDate);
    }
}
    
public record GraphicAccountResponse(Guid Id, string AccountNumber, string Type, DateTime CreationDate);