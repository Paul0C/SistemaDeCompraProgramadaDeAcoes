using CompraProgramada.Domain.Shared;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.ClientContext.ValueObjects;

public sealed record Cpf : ValueObject
{
    public string Number { get; }

    public Cpf(string number)
    {
        if (string.IsNullOrEmpty(number))
            throw new DomainException("The CPF is required.");

        var digits = new string(number.Where(char.IsDigit).ToArray());

        if(digits.Length != 11) throw new DomainException("CPF must have 11 digits.");
        if(!CpfValide(digits)) throw new DomainException("CPF invalid.");

        Number = digits;
    }

    public override string ToString()
        => Convert.ToUInt64(Number).ToString(@"000\.000\.000\-00");

    private static bool CpfValide(string cpf)
    {
        // Rejeitar CPFs repetidos
        if (new string(cpf[0], cpf.Length) == cpf)
            return false;

        int Soma(int length, int weight)
        {
            int sum = 0;
            for (int i = 0; i < length; i++)
                sum += (cpf[i] - '0') * (weight - i);
            return sum;
        }

        int dv1 = Soma(9, 10) % 11;
        dv1 = dv1 < 2 ? 0 : 11 - dv1;

        int dv2 = Soma(10, 11) % 11;
        dv2 = dv2 < 2 ? 0 : 11 - dv2;

        return cpf[9] - '0' == dv1 && cpf[10] - '0' == dv2;
    }
}