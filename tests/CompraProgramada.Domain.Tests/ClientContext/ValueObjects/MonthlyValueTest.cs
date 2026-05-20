using CompraProgramada.Domain.ClientContext.ValueObjects;

namespace tests.CompraProgramada.Domain.Tests.ClientContext.ValueObjects;

public class MonthlyValueTest
{
    [Fact]
    public void Create_WithValidValue_ShouldCreateMonthlyValue()
    {
        // Arrange
        decimal value = 150;

        // Act
        var monthlyValue = new MonthlyValue(value);

        // Assert
        monthlyValue.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithMinimumValue_ShouldCreateMonthlyValue()
    {
        // Arrange
        decimal value = MonthlyValue.MinimumValue;

        // Act
        var monthlyValue = new MonthlyValue(value);

        // Assert
        monthlyValue.Value.Should().Be(value);
    }

    [Fact]
    public void Create_WithValueLessThanMinimum_ShouldThrowException()
    {
        // Arrange
        decimal value = 99;

        // Act
        Action action = () => new MonthlyValue(value);

        // Assert
        action
            .Should()
            .Throw<Exception>()
            .WithMessage(
                $"Monthly value must be greater than or equal to {MonthlyValue.MinimumValue}.");
    }
}