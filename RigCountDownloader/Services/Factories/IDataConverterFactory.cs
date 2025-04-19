using RigCountDownloader.StreamProcessors;

namespace RigCountDownloader.Services.Factories;

public interface IDataConverterFactory
{
    IDataConverter CreateDataConverter(Response response);
}