using RigCountProcessor.Services.DataLoaders;

namespace RigCountProcessor.Services.Factories;

public class DataLoaderFactory(HttpClient httpClient) : IDataLoaderFactory
{
    public IDataLoader CreateDataLoader(string sourceType)
    {
        if (sourceType == SourceType.Http)
        {
            return new HttpDataLoader(httpClient);
        }

        throw new IncorrectSettingsException("Wrong or missing SourceType setting. Check appsettings.json file.");
    }
}