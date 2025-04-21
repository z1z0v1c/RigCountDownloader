namespace RigCountDownloader.Domain.Interfaces.Factories;

public interface IFileWriterFactory
{
    IFileWriter CreateFileWriter(string fireFormat, string fileLocation);
}