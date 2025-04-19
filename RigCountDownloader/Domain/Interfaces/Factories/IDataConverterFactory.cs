using RigCountDownloader.Domain.Interfaces.DataConverters;

namespace RigCountDownloader.Domain.Interfaces.Factories;

public interface IDataConverterFactory
{
    IDataConverter CreateDataConverter(string mediaType);
}