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
        private readonly ILogger _logger = logger;
        private readonly IConfiguration _configuration = configuration;
        private readonly Pipeline _pipeline = pipeline;

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            // TODO Implement cancellation logic
            try
            {
                Settings settings = new(
                    SourceType: _configuration["SourceType"]!,
                    SourceFileLocation: _configuration["SourceFileLocation"]!,
                    SourceFileFormat: _configuration["SourceFileFormat"]!,
                    OutputFileLocation: _configuration["OutputFileLocation"]!,
                    OutputFileFormat: _configuration["OutputFileFormat"]!
                );

                await pipeline.ExecuteAsync(settings, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error($"An exception occurred: {ex.Message}");
                _logger.Error($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}