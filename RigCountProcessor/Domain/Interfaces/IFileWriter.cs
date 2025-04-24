namespace RigCountProcessor.Domain.Interfaces;

public interface IFileWriter : IDisposable, IAsyncDisposable
{
    Task WriteLineAsync(string line, CancellationToken cancellationToken);
}