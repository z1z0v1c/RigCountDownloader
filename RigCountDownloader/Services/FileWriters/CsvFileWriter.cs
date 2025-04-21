using RigCountDownloader.Domain.Interfaces;

namespace RigCountDownloader.Services.FileWriters;

public class CsvFileWriter(string fileLocation) : IFileWriter
{
    public string FileLocation { get; } = fileLocation;

    private StreamWriter StreamWriter { get; } = new(fileLocation);

    public async Task WriteLineAsync(string line, CancellationToken cancellationToken)
    {
        await StreamWriter.WriteLineAsync(line);
    }

    public void Dispose() => StreamWriter.Dispose();

    public async ValueTask DisposeAsync() => await StreamWriter.DisposeAsync();
}