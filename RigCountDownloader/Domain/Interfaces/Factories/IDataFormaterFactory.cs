namespace RigCountDownloader.Domain.Interfaces.Factories;

public interface IDataFormaterFactory
{
    IDataFormater CreateDataFormater(string mediaType);
}