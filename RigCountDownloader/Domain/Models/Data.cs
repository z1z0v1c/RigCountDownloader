namespace RigCountDownloader
{
    public record Data(
        string? MediaType,
        string? SourceFileName,
        MemoryStream MemoryStream
    );
}