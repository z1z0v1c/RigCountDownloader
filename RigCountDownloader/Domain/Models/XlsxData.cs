using RigCountDownloader.Domain.Interfaces;

namespace RigCountDownloader.Domain.Models;

public record XlsxData(string FileType, string FileName, object Data) : IConvertedData;
