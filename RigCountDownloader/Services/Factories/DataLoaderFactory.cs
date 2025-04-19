using RigCountDownloader.Domain.Interfaces.DataLoaders;
using RigCountDownloader.Domain.Interfaces.Factories;
using RigCountDownloader.Domain.Models;
using RigCountDownloader.Services.DataLoaders;
using Serilog;

namespace RigCountDownloader.Services.Factories;

public class DataLoaderFactory(ILogger logger, HttpClient httpClient) : IDataLoaderFactory
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