namespace RigCountDownloader.Domain.Models
{
    public record Data(
        string? MediaType,
        string? FileName,
        MemoryStream MemoryStream
    ) : IDisposable, IAsyncDisposable
    {
        public void Dispose() => MemoryStream.Dispose();

        public async ValueTask DisposeAsync() => await MemoryStream.DisposeAsync();
    }
}