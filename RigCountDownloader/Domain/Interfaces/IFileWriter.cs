namespace RigCountDownloader.Domain.Interfaces;

public interface IFileWriter : IDisposable, IAsyncDisposable
{
    string FileLocation { get; set; }
    Task WriteLineAsync(string line, CancellationToken cancellationToken);
}