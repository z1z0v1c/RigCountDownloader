namespace RigCountProcessor.Tests.Mocks;

public class MockFileWriter(MemoryStream memoryStream) : IFileWriter
{
    private StreamWriter StreamWriter { get; } = new(memoryStream);

    public async Task WriteLineAsync(string line, CancellationToken cancellationToken)
    {
        await StreamWriter.WriteLineAsync(line);
    }

    public void Dispose() => StreamWriter.Dispose();

    public async ValueTask DisposeAsync() => await StreamWriter.DisposeAsync();
}
