namespace RigCountDownloader.Domain.Models;

public record Settings(
    string SourceType,
    string SourceFileLocation,
    string SourceFileFormat,
    string OutputFileLocation,
    string OutputFileFormat
);