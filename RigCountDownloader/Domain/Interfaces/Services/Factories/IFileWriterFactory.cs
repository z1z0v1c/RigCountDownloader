namespace RigCountDownloader.Domain.Interfaces.Services.Factories;

public interface IFileWriterFactory
{
    IFileWriter CreateFileWriter(string fireFormat, string fileLocation);
}