using Microsoft.Extensions.Configuration;
using RigCountDownloader.Domain.Models;
using RigCountDownloader.StreamProcessors;
using Serilog;

namespace RigCountDownloader.Application
{
    public class Application(
        ILogger logger,
        IConfiguration configuration,
        HttpDataLoader httpDataLoader,
        StreamProcessorFactory fileServiceFactory
    )
    {
        private readonly ILogger _logger = logger;
        private readonly IConfiguration _configuration = configuration;
        private readonly HttpDataLoader _httpDataLoader = httpDataLoader;
        private readonly StreamProcessorFactory _streamProcessorFactory = fileServiceFactory;

        public async Task RunAsync()
        {
            try
            {
                Settings settings = new(
                    SourceType: _configuration["SourceType"]!,
                    SourceFileLocation: _configuration["SourceFileLocation"]!,
                    SourceFileFormat: _configuration["SourceFileFormat"]!,
                    OutputFileLocation: _configuration["OutputFileLocation"]!,
                    OutputFileFormat: _configuration["OutputFileFormat"]!
                );
                    
                _logger.Information($"Retrieving source file from {settings.SourceFileLocation}...");

                var response = await _httpDataLoader.DownloadFileAsStreamAsync(new(settings.SourceFileLocation));

                _logger.Information($"Download completed successfully. Received {response.MemoryStream.Length} bytes.");

                await using Stream fileStream = response.MemoryStream;

                IStreamProcessor streamProcessor = _streamProcessorFactory.CreateStreamProcessor(response);

                await streamProcessor.ProcessStreamAsync(fileStream);
            }
            catch (Exception ex)
            {
                _logger.Error($"An exception occurred: {ex.Message}");
                _logger.Error($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}