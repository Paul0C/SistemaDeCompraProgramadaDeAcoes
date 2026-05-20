using CompraProgramada.Domain.ClientContext.ValueObjects;

namespace tests.CompraProgramada.Domain.Tests.ClientContext.ValueObjects;

public class AccountNumberTest
{
    [Fact]
    public void Create_ShouldGenerateAccountNumber()
    {
        // Act
        var accountNumber = new AccountNumber();

        // Assert
        accountNumber.Number.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Create_ShouldStartWithCG()
    {
        // Act
        var accountNumber = new AccountNumber();

        // Assert
        accountNumber.Number.Should().StartWith("CG");
    }

    [Fact]
    public void Create_ShouldHaveExpectedLength()
    {
        // Act
        var accountNumber = new AccountNumber();

        // Assert
        accountNumber.Number.Length.Should().Be(20);
    }

    [Fact]
    public void Create_ShouldGenerateDifferentValues()
    {
        // Act
        var first = new AccountNumber();
        var second = new AccountNumber();

        // Assert
        first.Number.Should().NotBe(second.Number);
    }
}