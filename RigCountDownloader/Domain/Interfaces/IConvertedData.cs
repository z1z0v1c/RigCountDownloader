namespace RigCountDownloader.Domain.Interfaces;

public interface IConvertedData
{
   string FileType { get; }
   string FileName { get; }
   object Data { get; }
}