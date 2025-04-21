using RigCountDownloader.Domain.Interfaces;

namespace RigCountDownloader.Domain.Models;

public record XlsxData(string FileFormat, string FileName, object Data) : IConvertedData;
