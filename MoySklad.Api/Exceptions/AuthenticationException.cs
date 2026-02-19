namespace MoySklad.Api.Exceptions;

public class AuthenticationException : MoySkladException
{
    public AuthenticationException(string message) : base(401, message)
    {
    }

    public AuthenticationException(string message, string? responseBody)
        : base(401, message, responseBody)
    {
        
    }

    public AuthenticationException(string message, Exception innerException)
        : base(401, message, innerException)
    {
    }
}