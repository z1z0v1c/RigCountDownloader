namespace RigCountDownloader.Domain.Interfaces.Factories;

public interface IDataConverterFactory
{
    IDataConverter CreateDataConverter(string mediaType);
}