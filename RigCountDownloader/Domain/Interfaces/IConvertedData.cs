namespace RigCountDownloader.Domain.Interfaces;

public interface IConvertedData
{
   string FileFormat { get; }
   string FileName { get; }
   object Data { get; }
}