namespace RigCountDownloader.Domain.Interfaces.Services.Factories;

public interface IDataConverterFactory
{
    IDataConverter CreateDataConverter(string mediaType);
}