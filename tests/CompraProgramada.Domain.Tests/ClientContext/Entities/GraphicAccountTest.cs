using CompraProgramada.Domain.ClientContext.Entities;
using CompraProgramada.Domain.ClientContext.Enums;
using CompraProgramada.Domain.ClientContext.Events;

namespace tests.CompraProgramada.Domain.Tests.ClientContext.Entities;

public class GraphicAccountTest
{
    [Fact]
    public void Create_ShouldSetTypeAsBaby()
    {
        // Act
        var graphicAccount = GraphicAccount.Create(Guid.NewGuid());

        // Assert
        graphicAccount.Type.Should().Be(AccountType.Baby);
        graphicAccount.AccountNumber.Should().NotBeNull();
        graphicAccount.ClientId.Should().NotBeEmpty();
        graphicAccount.CreateDate.Should().NotBe(DateTime.MinValue);
    }
    
    [Fact]
    public void Create_ShouldAddDomainEvent()
    {
        // Act
        var graphicAccount = GraphicAccount.Create(Guid.Empty);

        // Assert
        graphicAccount.DomainEvents.Should().ContainSingle();

        graphicAccount.DomainEvents
            .Should()
            .Contain(e => e is OnGraphicAccountCreatedEvent);
    }

    [Fact]
    public void Create_ShouldAddCorrectDomainEvent()
    {
        // Act
        var graphicAccount = GraphicAccount.Create(Guid.Empty);

        // Assert
        var domainEvent = graphicAccount.DomainEvents
            .OfType<OnGraphicAccountCreatedEvent>()
            .Single();

        domainEvent.GraphicAccountId.Should().Be(graphicAccount.Id);
    }
}