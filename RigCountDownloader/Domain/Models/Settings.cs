namespace RigCountDownloader.Domain.Models;

public record Settings(
    string SourceType,
    string SourceFileLocation,
    string OutputFileLocation,
    string OutputFileFormat
);