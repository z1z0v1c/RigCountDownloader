using RigCountDownloader.Domain.Interfaces;
using RigCountDownloader.Domain.Interfaces.Factories;
using RigCountDownloader.Domain.Interfaces.Services.Factories;
using RigCountDownloader.Domain.Models;
using Serilog;

namespace RigCountDownloader.Application;

public class Pipeline(
    IDataLoaderFactory dataLoaderFactory,
    IDataProcessorFactory dataProcessorFactory,
    IDataFormaterFactory dataFormaterFactory,
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
        await using DataStream dataStream = await dataLoader.LoadDataAsync(settings.SourceFileLocation, cancellationToken);

        logger.Information("Data retrieved successfully. Received {MemoryStreamLength} bytes.", dataStream.MemoryStream.Length);

        // Create data converter
        IDataFormater dataFormater = dataFormaterFactory.CreateDataFormater(dataStream.MediaType);

        logger.Information("Formating {MediaType} data stream...", dataStream.MediaType);

        // Format data
        FormatedData convertedData = dataFormater.FormatData(dataStream);

        logger.Information("Data formated successfully into {SourceFileFormat} format.", convertedData.FileFormat);

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