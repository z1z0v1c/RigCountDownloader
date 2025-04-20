namespace RigCountDownloader.Domain.Interfaces.Factories;

public interface IFileWriterFactory
{
    IFileWriter CreateFileWriter(string fileType, string fileLocation);
}