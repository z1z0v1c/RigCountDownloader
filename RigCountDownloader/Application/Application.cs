using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace RigCountDownloader.Application
{
    public class Application(
        ILogger logger,
        IConfiguration configuration,
        Pipeline pipeline
    )
    {
        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            logger.Information("Starting application...");
            // TODO Implement cancellation logic
            logger.Information("Reading settings from the appsettings.json file...");
            try
            {
                Settings settings = new(
                    Context: configuration["Context"]!,
                    SourceType: configuration["SourceType"]!,
                    SourceFileLocation: configuration["SourceFileLocation"]!,
                    OutputFileLocation: configuration["OutputFileLocation"]!,
                    OutputFileFormat: configuration["OutputFileFormat"]!
                );

                logger.Information("Starting data processing pipeline with settings: {Settings}",
                    JsonSerializer.Serialize(settings));

                await pipeline.ExecuteAsync(settings, cancellationToken);

                logger.Information("Closing application...");
            }
            catch (Exception exception)
            {
                logger.Error("An exception has occured: {Exception}", exception);
                Environment.Exit(1);
            }
        }
    }
}