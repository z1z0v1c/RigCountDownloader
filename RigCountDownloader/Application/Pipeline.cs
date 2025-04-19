using System.Text.Json;
using Serilog;
using RigCountDownloader.Domain.Interfaces.Factories;
using RigCountDownloader.Domain.Models;
using RigCountDownloader.Services.Factories;

namespace RigCountDownloader.Application;

public class Pipeline(
    IDataLoaderFactory dataLoaderFactory,
    IDataProcessorFactory dataProcessorFactory,
    IDataConverterFactory dataConverterFactory,
    ILogger logger
)
{
    public async Task ExecuteAsync(Settings settings, CancellationToken cancellationToken = default)
    {
        logger.Information("Starting data processing pipeline with options: {Options}",
            JsonSerializer.Serialize(settings));

        try
        {
            // Load data
            var dataLoader = dataLoaderFactory.CreateDataLoader(settings);
            logger.Information($"Retrieving data from the source: {settings.SourceFileLocation}");
            logger.Information($"Retrieving source file from {settings.SourceFileLocation}...");
            using var data = await dataLoader.LoadDataAsync(new Uri(settings.SourceFileLocation!));
            logger.Information($"Download completed successfully. Received {data.MemoryStream.Length} bytes.");
            
            // Convert data
            var dataConverter = dataConverterFactory.CreateDataConverter(data.MediaType!);
            logger.Information("Converting data...");
            var convertedData = await dataConverter.ConvertDataAsync(data);

            // Process and save data
            var processor = dataProcessorFactory.CreateDataProcessor(convertedData);
            logger.Information("Processing data...");
            // cancellationToken?
             await processor.ProcessAndSaveAsync();

            logger.Information("Data processing completed successfully");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error during data processing");
            throw;
        }
    }
}
