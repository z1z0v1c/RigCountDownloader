using Microsoft.Extensions.Configuration;
using RigCountDownloader.Domain.Models;
using Serilog;

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
            // TODO Implement cancellation logic
            try
            {
                Settings settings = new(
                    Context:            configuration["Context"]!,
                    SourceType:         configuration["SourceType"]!,
                    SourceFileLocation: configuration["SourceFileLocation"]!,
                    OutputFileLocation: configuration["OutputFileLocation"]!,
                    OutputFileFormat:   configuration["OutputFileFormat"]!
                );

                await pipeline.ExecuteAsync(settings, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error($"An exception has occurred: {ex.Message}");
                logger.Error($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}