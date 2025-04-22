namespace RigCountDownloader.Domain.Interfaces;

public interface IFileWriter : IDisposable, IAsyncDisposable
{
    string FileLocation { get; }

    Task WriteLineAsync(string line, CancellationToken cancellationToken);
}