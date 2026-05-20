using CompraProgramada.Domain.ClientContext.ValueObjects;
using CompraProgramada.Domain.SharedContext;

namespace tests.CompraProgramada.Domain.Tests.ClientContext.ValueObjects;

public class CpfTest
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CpfConstructor_WithCpfInvalid_ShouldThrowDomainException(string cpf)
    {
        //Act
        Action act = () => new Cpf(cpf);
        
        //Assert
        act.Should().Throw<DomainException>().And.Message.Should().Be($"The CPF is required.");
    }
    
    [Theory]
    [InlineData("49055541070")]
    [InlineData("49057811170")]
    [InlineData("00000000000")]
    public void CpfValide_WithCpfInvalid_ShouldThrowDomainException(string cpf)
    {
        //Act
        Action act = () => new Cpf(cpf);
        
        //Assert
        act.Should().Throw<DomainException>().And.Message.Should().Be($"CPF invalid.");
    }

    public void CpfToString_WithCpfValid_ShouldReturnTheCorrectCpf()
    {
        //Act
        var cpf = new Cpf("49057841070");
        
        //Act
        var result = cpf.ToString();
        
        //Assert
        result.Should().Be("490.578.410-70");
    }
}