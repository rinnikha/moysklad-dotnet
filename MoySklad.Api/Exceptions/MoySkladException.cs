namespace MoySklad.Api.Exceptions;

public class MoySkladException : Exception
{
    public int? StatusCode { get; }
    public string? ResponseBody { get; }

    public MoySkladException(string message)
        : base(message)
    {
    }

    public MoySkladException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
    
    public MoySkladException(int statusCode, string message, Exception innerException)
        : base($"MoySklad API Error: {statusCode} - {message}", innerException)
    {}

    public MoySkladException(int statusCode, string message, string? responseBody = null)
        : base($"MoySklad API Error: {statusCode} - {message}")
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }
}