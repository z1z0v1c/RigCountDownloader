namespace RigCountDownloader.Domain.Models.Exceptions;

public class HttpDataLoadException : Exception
{
    public HttpDataLoadException(string message) : base(message)
    {
    }

    public HttpDataLoadException(string message, Exception innerException) : base(message, innerException)
    {
    }
}