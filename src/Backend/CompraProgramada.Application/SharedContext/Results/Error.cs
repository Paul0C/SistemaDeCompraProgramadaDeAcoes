namespace CompraProgramada.Application.SharedContext.Results;

public record Error(string Code, string Message)
{
    public static Error None = new(string.Empty, string.Empty);
    public static Error NullValue = new("Error.NullValue", "A value null was provided.");
}