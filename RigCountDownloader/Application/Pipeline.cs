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
        // Create data loader
        IDataLoader dataLoader = dataLoaderFactory.CreateDataLoader(settings.SourceType);

        logger.Information("Retrieving data from the {SourceType} source: {SourceFileLocation}",
            settings.SourceType, settings.SourceFileLocation);

        // Load data
        await using Data data = await dataLoader.LoadDataAsync(settings.SourceFileLocation, cancellationToken);

        logger.Information("Data retrieved successfully. Received {MemoryStreamLength} bytes.", data.MemoryStream.Length);

        // Create data converter
        IDataConverter dataConverter = dataConverterFactory.CreateDataConverter(data.MediaType);

        logger.Information("Converting {MediaType} data stream into formated data...", data.MediaType);

        // Convert data
        IConvertedData convertedData = dataConverter.ConvertData(data);

        logger.Information("Data converted successfully into {SourceFileFormat} format.", convertedData.FileFormat);

        // Create data processor and file writer
        IFileWriter fileWriter = fileWriterFactory.CreateFileWriter(
            settings.OutputFileFormat, settings.OutputFileLocation);
        IDataProcessor processor = dataProcessorFactory.CreateDataProcessor(
            fileWriter, settings.Context, convertedData.FileFormat, convertedData.Data);

        logger.Information("Starting {SourceFileFormat} data processing...", convertedData.FileFormat);

        // Process and save data
        await processor.ProcessAndSaveAsync(cancellationToken);

        logger.Information("Data processed and saved to a {OutputFileFormat} successfully!", settings.OutputFileFormat);
    }
}