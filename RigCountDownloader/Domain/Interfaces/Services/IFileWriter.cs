namespace RigCountDownloader.Domain.Interfaces.Services;

public interface IFileWriter : IDisposable, IAsyncDisposable
{
    string FileLocation { get; }
    
    Task WriteLineAsync(string line, CancellationToken cancellationToken);
}