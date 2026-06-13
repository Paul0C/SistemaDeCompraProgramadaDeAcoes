using System.ComponentModel.Design;
using CompraProgramada.Domain.ClientContext.ValueObjects;
using CompraProgramada.Domain.SharedContext;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.ClientContext.Entities;

public class Client : AggregateRoot
{
    public string Name { get; private set; }
    public Cpf CPF { get; private set; }
    public Email Email { get; private set; }
    public MonthlyValue MonthlyValue { get; private set; }
    public bool Active { get; private set; }
    public DateTime AdhesionDate { get; private set; }
    public GraphicAccount GraphicAccount { get; private set; }

    private Client() : base()
    {
        
    }

    private Client(string name, Cpf cpf, Email email, MonthlyValue monthlyValue) : base()
    {
        Name = name;
        CPF = cpf;
        Email = email;
        MonthlyValue = monthlyValue;
        AdhesionDate = DateTime.Now;
        Active =  true;
    }
    
    public static Client Create(string name, Cpf cpf, Email email, MonthlyValue monthlyValue)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name)) throw new DomainException("The client name is required.");
        if(cpf is null) throw new DomainException("The CPF is required.");
        if (email is null) throw new DomainException("The email is required.");
        if (monthlyValue is null) throw new DomainException("The monthly value is required.");
        
        var client = new Client(name, cpf, email, monthlyValue);
        client.GraphicAccount = GraphicAccount.Create(client.Id);
        return client;
    }

    public void Exit()
    {
        if(Active)
            Active = !Active;
    }
}