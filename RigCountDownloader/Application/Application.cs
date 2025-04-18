using Microsoft.Extensions.Configuration;
using RigCountDownloader.StreamProcessors;
using Serilog;

namespace RigCountDownloader.Application
{
    public class Application(
        ILogger logger,
        IConfiguration configuration,
        StreamDownloader streamDownloader,
        StreamProcessorFactory fileServiceFactory
    )
    {
        private readonly ILogger _logger = logger;
        private readonly IConfiguration _configuration = configuration;
        private readonly StreamDownloader _streamDownloader = streamDownloader;
        private readonly StreamProcessorFactory _streamProcessorFactory = fileServiceFactory;

        public async Task RunAsync()
        {
            string uriString = _configuration["InputFileUri"] ?? string.Empty;
            if (string.IsNullOrEmpty(uriString))
            {
                throw new ArgumentException("InputFileUri configuration value is missing or empty.");
            }

            Uri uri = new(uriString);

            try
            {
                _logger.Information($"Downloading file from {uri}...");

                var response = await _streamDownloader.DownloadFileAsStreamAsync(uri);

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