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
                    configuration["SourceType"]!,
                    configuration["SourceFileLocation"]!,
                    configuration["OutputFileLocation"]!,
                    configuration["OutputFileFormat"]!
                );

                await pipeline.ExecuteAsync(settings, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error($"An exception occurred: {ex.Message}");
                logger.Error($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}