namespace MoySklad.Api.Exceptions;

public class ValidationException : MoySkladException
{
    public List<ValidationError>? ValidationErrors { get; }
    
    public ValidationException(string message)
        : base(400, message)
    {}
    
    public ValidationException(string message, string? responseBody)
        : base(400, message, responseBody)
    {}

    public ValidationException(int statusCode, string message, string? responseBody = null)
        : base(statusCode, message, responseBody)
    {
    }

    public ValidationException(string message, List<ValidationError> validationErrors)
        : base(400, message)
    {
        ValidationErrors = validationErrors;
    }
    
    public ValidationException(string message, Exception innerException)
        : base(400, message, innerException)
    {}
}

public record ValidationError
{
    public string? Error { get; init; }
    public string? Parameter { get; init; }
    public int? Code { get; init; }
    public string? Href { get; init; }
}