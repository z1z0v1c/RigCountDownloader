namespace RigCountDownloader
{
    public record Response(
        string? MediaType,
        string? FileName,
        MemoryStream MemoryStream
    );
}