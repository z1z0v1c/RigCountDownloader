using RigCountDownloader.Domain.Interfaces;
using RigCountDownloader.Domain.Interfaces.Services.Factories;
using RigCountDownloader.Domain.Models.Enums;
using RigCountDownloader.Services.DataLoaders;
using Serilog;

namespace RigCountDownloader.Services.Factories;

public class DataLoaderFactory(ILogger logger, HttpClient httpClient) : IDataLoaderFactory
{
    public IDataLoader CreateDataLoader(string sourceType)
    {
        if (sourceType == SourceType.Http)
        {
            return new HttpDataLoader(logger, httpClient);
        }

        throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
    } 
}