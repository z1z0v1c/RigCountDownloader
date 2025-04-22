using RigCountDownloader.Services.DataLoaders;

namespace RigCountDownloader.Services.Factories;

public class DataLoaderFactory(ILogger logger, HttpClient httpClient) : IDataLoaderFactory
{
    public IDataLoader CreateDataLoader(string sourceType)
    {
        if (sourceType == SourceType.Http)
        {
            return new HttpDataLoader(logger, httpClient);
        }

        throw new IncorrectSettingsException("Wrong or missing SourceType setting. Check appsettings.json file.");
    }
}