using System.Text.Json;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace RigCountProcessor.Application;

public class Application(
    ILogger logger,
    IConfiguration configuration,
    Pipeline pipeline
)
{
    public async Task RunAsync(string[] args, CancellationToken cancellationToken = default)
    {
        logger.Information("Starting application...");

        // TODO Implement cancellation logic
        try
        {
            logger.Information("Parsing command line arguments...");

            // Parse cmd args as Options
            var options = Parser.Default.ParseArguments<Options>(args).Value;

            logger.Information("Reading settings from the appsettings.json file...");

            // Import settings
            Settings settings = new(
                Context: configuration["Context"]!,
                SourceType: configuration["SourceType"]!,
                SourceFileLocation: configuration["SourceFileLocation"]!,
                OutputFileLocation: configuration["OutputFileLocation"]!,
                OutputFileFormat: configuration["OutputFileFormat"]!
            );

            logger.Information(
                "Starting data processing pipeline with settings: {Settings} and options: {Options}.",
                JsonSerializer.Serialize(settings), JsonSerializer.Serialize(options));

            // Execute data processing pipeline
            await pipeline.ExecuteAsync(settings, options, cancellationToken);

            logger.Information("Closing application...");
        }
        catch (Exception exception)
        {
            logger.Error("An exception has occured: {Exception}", exception);
            Environment.Exit(1);
        }
    }
}