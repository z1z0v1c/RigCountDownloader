namespace RigCountDownloader.Domain.Models.Exceptions;

public class HttpDataLoadException(string message, Exception innerException) : Exception(message, innerException);