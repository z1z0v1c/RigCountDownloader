namespace RigCountDownloader.Domain.Interfaces;

public interface IConvertedData
{
   string FileType { get; init; }
   string FileName { get; init; }
   object Data { get; init; }
}