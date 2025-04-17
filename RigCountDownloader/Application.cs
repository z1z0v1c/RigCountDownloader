using RigCountDownloader.StreamProcessors;
using Serilog;

namespace RigCountDownloader
{
	public class Application(ILogger logger, StreamDownloader streamDownloader, StreamProcessorFactory fileServiceFactory)
    {
		private readonly ILogger _logger = logger;
		private readonly StreamDownloader _streamDownloader = streamDownloader;
		private readonly StreamProcessorFactory _streamProcessorFactory = fileServiceFactory;

        public async Task RunAsync()
		{
			try
			{
				using Stream fileStream = await _streamDownloader.DownloadFileAsStreamAsync();

				IStreamProcessor streamProcessor = _streamProcessorFactory.CreateStreamProcessor();
				await streamProcessor.ProcessStreamAsync(fileStream);
			}
			catch (Exception ex)
			{
				_logger.Error("An exception occurred: " + ex.Message);
				_logger.Error("Stack Trace: " + ex.StackTrace);
			}
		}
	}
}
