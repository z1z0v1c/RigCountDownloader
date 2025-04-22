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
        await using DataStream dataStream =
            await dataLoader.LoadDataAsync(settings.SourceFileLocation, cancellationToken);

        logger.Information("Data retrieved successfully. Received {MemoryStreamLength} bytes.",
            dataStream.MemoryStream.Length);

        // Create data converter
        IDataFormatter dataFormatter = dataFormaterFactory.CreateDataFormatter(dataStream.MediaType);

        logger.Information("Formating {MediaType} data stream...", dataStream.MediaType);

        // Format data
        FormattedData formattedData = dataFormatter.FormatData(dataStream);

        logger.Information("Data formatted successfully into {SourceFileFormat} format.", formattedData.Format);

        // Create data processor and file writer
        IFileWriter fileWriter = fileWriterFactory.CreateFileWriter(
            settings.OutputFileFormat, settings.OutputFileLocation);
        IDataProcessor processor = dataProcessorFactory.CreateDataProcessor(
            fileWriter, settings.Context, formattedData.Format, formattedData.Data);

        logger.Information("Starting {SourceFileFormat} data processing...", formattedData.Format);

        // Process and save data
        await processor.ProcessAndSaveDataAsync(cancellationToken);

        logger.Information("Data processed and saved to a {OutputFileFormat} successfully!", settings.OutputFileFormat);
    }
}