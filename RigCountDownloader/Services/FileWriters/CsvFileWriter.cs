using RigCountDownloader.Domain.Interfaces;

namespace RigCountDownloader.Services.FileWriters;

public class CsvFileWriter : IFileWriter
{
    public CsvFileWriter()
    {
    }

    public CsvFileWriter(string fileLocation)
    {
        FileLocation = fileLocation;
        StreamWriter = new StreamWriter(fileLocation);
    }

    public string FileLocation { get; set;  }
    
    public StreamWriter StreamWriter { get; }

    public async Task WriteLineAsync(string line, CancellationToken cancellationToken)
    {
        await StreamWriter.WriteLineAsync(line);
    }

    public void Dispose() => StreamWriter.Dispose();

    public async ValueTask DisposeAsync() => await StreamWriter.DisposeAsync();
}