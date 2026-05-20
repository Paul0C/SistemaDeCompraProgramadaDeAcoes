using CompraProgramada.Domain.ClientContext.Entities;
using CompraProgramada.Domain.ClientContext.ValueObjects;
using CompraProgramada.Domain.SharedContext;
using FluentAssertions;

namespace tests.CompraProgramada.Domain.Tests.ClientContext.Entities;

public class ClientTest
{
    private static Client CreateValidClient(
        string name = "John Doe",
        string cpf = "49057841070",
        string email = "john@example.com",
        decimal monthlyValue = 200m) =>
        Client.Create(
            name,
            new Cpf(cpf),
            new Email(email),
            new MonthlyValue(monthlyValue));

    [Fact]
    public void Create_WithValidData_ShouldReturnClientWithCorrectProperties()
    {
        // Arrange
        var name = "John Doe";
        var cpf = new Cpf("49057841070");
        var email = new Email("john@example.com");
        var monthlyValue = new MonthlyValue(200m);

        // Act
        var client = Client.Create(name, cpf, email, monthlyValue);

        // Assert
        client.Name.Should().Be(name);
        client.CPF.Should().Be(cpf);
        client.Email.Should().Be(email);
        client.MonthlyValue.Should().Be(monthlyValue);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithNameInvalid_ShouldThrowDomainException(string name)
    {
        // Arrange
        var cpf = new Cpf("49057841070");
        var email = new Email("john@example.com");
        var monthlyValue = new MonthlyValue(200m);

        // Act
        Action act = () => Client.Create(name, cpf, email, monthlyValue);

        // Assert
        act.Should().Throw<DomainException>().And.Message.Should().Be("The client name is required.");
    }  
    [Fact]
    public void Create_WithValidData_ShouldSetActiveToTrue()
    {
        // Arrange & Act
        var client = CreateValidClient();

        // Assert
        client.Active.Should().BeTrue();
    }
    

    [Fact]
    public void Create_WithValidData_ShouldCreateGraphicAccount()
    {
        // Arrange & Act
        var client = CreateValidClient();

        // Assert
        client.GraphicAccount.Should().NotBeNull();
    }

    [Fact]
    public void Create_WithValidData_ShouldAssignUniqueGraphicAccountPerClient()
    {
        // Arrange & Act
        var clientA = CreateValidClient();
        var clientB = CreateValidClient(cpf: "49057841070", email: "jane@example.com");

        // Assert
        clientA.GraphicAccount.AccountNumber.Should().NotBe(clientB.GraphicAccount.AccountNumber);
    }
    
    [Theory]
    [InlineData("Cpf")]
    [InlineData("Email")]
    [InlineData("MonthlyValue")]
    public void Create_WithValueObjectsNull_ShouldThrowDomainException(string campo)
    {
        //Arrange
        var cpf = campo == "Cpf" ? null : new Cpf("49057841070");
        var email = campo == "Email" ? null : new Email("jane@example.com");
        var monthlyValue = campo == "MonthlyValue" ? null : new MonthlyValue(200m);
        
        //Act
        Action act = () => Client.Create("Paulo", cpf!, email!, monthlyValue!);
        
        //Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Exit_WhenClientIsActive_ShouldDeactivateClient()
    {
        // Arrange
        var client = CreateValidClient();

        // Act
        client.Exit();

        // Assert
        client.Active.Should().BeFalse();
    }

    [Fact]
    public void Exit_WhenClientIsAlreadyInactive_ShouldKeepClientInactive()
    {
        // Arrange
        var client = CreateValidClient();
        client.Exit();

        // Act
        client.Exit();

        // Assert
        client.Active.Should().BeFalse();
    }
}
