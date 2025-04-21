namespace RigCountDownloader.Domain.Models;

public record Settings(
    string Context,
    string SourceType,
    string SourceFileLocation,
    string OutputFileLocation,
    string OutputFileFormat
);