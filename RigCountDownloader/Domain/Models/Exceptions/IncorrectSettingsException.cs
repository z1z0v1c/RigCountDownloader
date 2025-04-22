namespace RigCountDownloader.Domain.Models.Exceptions;

public class IncorrectSettingsException(string message) : Exception
{
    public override string Message { get; } = message;
}