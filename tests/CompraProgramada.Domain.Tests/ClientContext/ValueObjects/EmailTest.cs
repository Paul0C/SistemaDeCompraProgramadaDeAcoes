using CompraProgramada.Domain.ClientContext.ValueObjects;
using CompraProgramada.Domain.SharedContext;
using FluentAssertions;

namespace tests.CompraProgramada.Domain.Tests.ClientContext.ValueObjects;

public class EmailTest
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("user.name@domain.org")]
    [InlineData("user-name@sub.domain.com")]
    [InlineData("USER@EXAMPLE.COM")]
    public void Constructor_WithValidAddress_ShouldCreateEmail(string address)
    {
        // Act
        var email = new Email(address);

        // Assert
        email.Address.Should().Be(address.Trim().ToLowerInvariant());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_WithNullOrEmptyAddress_ShouldThrowDomainException(string address)
    {
        // Act
        Action act = () => new Email(address);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The email is required.");
    }

    [Theory]
    [InlineData("notanemail")]
    [InlineData("missing@tld")]
    [InlineData("@nodomain.com")]
    [InlineData("spaces in@email.com")]
    [InlineData("double@@domain.com")]
    public void Constructor_WithInvalidFormat_ShouldThrowDomainException(string address)
    {
        // Act
        Action act = () => new Email(address);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Email invalid.");
    }

    [Theory]
    [InlineData("  user@example.com  ")]
    [InlineData("  USER@EXAMPLE.COM  ")]
    public void Constructor_WithAddressContainingWhitespace_ShouldTrimAndNormalize(string address)
    {
        // Act
        var email = new Email(address);

        // Assert
        email.Address.Should().Be(address.Trim().ToLowerInvariant());
    }

    [Fact]
    public void Constructor_WithUpperCaseAddress_ShouldNormalizeToLowerCase()
    {
        // Arrange
        var address = "USER@EXAMPLE.COM";

        // Act
        var email = new Email(address);

        // Assert
        email.Address.Should().Be("user@example.com");
    }

    [Fact]
    public void Equality_WithSameAddress_ShouldBeEqual()
    {
        // Arrange
        var emailA = new Email("user@example.com");
        var emailB = new Email("user@example.com");

        // Assert
        emailA.Should().Be(emailB);
    }

    [Fact]
    public void Equality_WithSameAddressDifferentCase_ShouldBeEqual()
    {
        // Arrange
        var emailA = new Email("user@example.com");
        var emailB = new Email("USER@EXAMPLE.COM");

        // Assert
        emailA.Should().Be(emailB);
    }

    [Fact]
    public void Equality_WithDifferentAddresses_ShouldNotBeEqual()
    {
        // Arrange
        var emailA = new Email("user@example.com");
        var emailB = new Email("other@example.com");

        // Assert
        emailA.Should().NotBe(emailB);
    }

    [Fact]
    public void ToString_ShouldReturnNormalizedAddress()
    {
        // Arrange
        var email = new Email("USER@EXAMPLE.COM");

        // Act
        var result = email.ToString();

        // Assert
        result.Should().Be("user@example.com");
    }
}
