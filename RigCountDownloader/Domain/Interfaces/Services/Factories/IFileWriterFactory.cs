namespace RigCountDownloader.Domain.Interfaces.Services.Factories;

public interface IFileWriterFactory
{
    IFileWriter CreateFileWriter(string fileType, string fileLocation);
}