namespace RigCountProcessor.Domain.Models;

public record DataStream(
    string MediaType,
    MemoryStream MemoryStream
) : IDisposable, IAsyncDisposable
{
    public void Dispose() => MemoryStream.Dispose();

    public async ValueTask DisposeAsync() => await MemoryStream.DisposeAsync();
}