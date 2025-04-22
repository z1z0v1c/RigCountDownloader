namespace RigCountDownloader.Domain.Interfaces.Factories;

public interface IDataFormaterFactory
{
    IDataFormatter CreateDataFormatter(string mediaType);
}