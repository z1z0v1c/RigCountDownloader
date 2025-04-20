using System.Text.Json;
using Serilog;
using RigCountDownloader.Domain.Interfaces.Factories;
using RigCountDownloader.Domain.Models;

namespace RigCountDownloader.Application;

public class Pipeline(
    IDataLoaderFactory dataLoaderFactory,
    IDataProcessorFactory dataProcessorFactory,
    IDataConverterFactory dataConverterFactory,
    ILogger logger
)
{
    public async Task ExecuteAsync(Settings settings, CancellationToken cancellationToken)
    {
        logger.Information($"Starting data processing pipeline with options: {JsonSerializer.Serialize(settings)}");

        try
        {
            var dataLoader = dataLoaderFactory.CreateDataLoader(settings, cancellationToken);
            // Load data
            logger.Information($"Retrieving data from the source: {settings.SourceFileLocation}");
            using var data = await dataLoader.LoadDataAsync(new Uri(settings.SourceFileLocation!), cancellationToken);
            logger.Information($"Data retreived successfully. Received {data.MemoryStream.Length} bytes.");
            
            var dataConverter = dataConverterFactory.CreateDataConverter(data.MediaType!);
            // Convert data
            logger.Information("Converting data...");
            var convertedData = dataConverter.ConvertData(data);
            logger.Information("Data converted successfully.");

            var processor = dataProcessorFactory.CreateDataProcessor(convertedData);
            // Process and save data
            logger.Information("Processing data...");
            await processor.ProcessAndSaveAsync(cancellationToken);
            logger.Information("Data processed and saved successfully!");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error during data processing");
            throw;
        }
    }
}
