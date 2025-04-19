using RigCountDownloader.Domain.Models;
using RigCountDownloader.StreamProcessors;
using Serilog;

namespace RigCountDownloader.Services.Factories;

public class DataLoaderFactory(ILogger logger, HttpClient httpClient)
{
    public IDataLoader CreateDataLoader(Settings settings)
    {
        if (settings.SourceType == "http")
        {
            return new HttpDataLoader(logger, httpClient);
        }

        throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
    } 
}