using System.Text.Json;
using RigCountDownloader.Domain.Interfaces;
using RigCountDownloader.Domain.Interfaces.Services;
using RigCountDownloader.Domain.Interfaces.Services.Factories;
using RigCountDownloader.Domain.Models;
using Serilog;

namespace RigCountDownloader.Application;

public class Pipeline(
    IDataLoaderFactory dataLoaderFactory,
    IDataProcessorFactory dataProcessorFactory,
    IDataConverterFactory dataConverterFactory,
    IFileWriterFactory fileWriterFactory,
    ILogger logger
)
{
    public async Task ExecuteAsync(Settings settings, CancellationToken cancellationToken)
    {
        logger.Information($"Starting data processing with options: {JsonSerializer.Serialize(settings)}");

        IDataLoader dataLoader = dataLoaderFactory.CreateDataLoader(settings.SourceType);

        // Load data
        logger.Information($"Retrieving data from the source: {settings.SourceFileLocation}");
        await using Data data = await dataLoader.LoadDataAsync(settings.SourceFileLocation, cancellationToken);
        logger.Information($"Data retrieved successfully. Received {data.MemoryStream.Length} bytes.");

        IDataConverter dataConverter = dataConverterFactory.CreateDataConverter(data.MediaType);

        // Convert data
        logger.Information("Converting data...");
        IConvertedData convertedData = dataConverter.ConvertData(data);
        logger.Information("Data converted successfully.");

        IFileWriter fileWriter = fileWriterFactory.CreateFileWriter(
            settings.OutputFileFormat, settings.OutputFileLocation);
        IDataProcessor processor = dataProcessorFactory.CreateDataProcessor(
            fileWriter, convertedData.FileFormat, convertedData.FileName, convertedData.Data);

        // Process and save data
        logger.Information("Processing data...");
        await processor.ProcessAndSaveAsync(cancellationToken);
        logger.Information("Data processed and saved successfully!");
    }
}