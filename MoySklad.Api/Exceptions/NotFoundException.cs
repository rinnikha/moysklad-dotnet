namespace MoySklad.Api.Exceptions;

public class NotFoundException : MoySkladException
{
    public NotFoundException(string message) : base(404, message)
    {}
    
    public NotFoundException(string message, string? responseBody)
        : base(404, message, responseBody)
    {}

    public NotFoundException(string message, Exception innerException)
        : base(404, message, innerException)
    {}
}
