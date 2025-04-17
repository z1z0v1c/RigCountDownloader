using RigCountDownloader.StreamProcessors;
using Serilog;

namespace RigCountDownloader
{
	public class Application
	{
		private readonly ILogger _logger;
		private readonly StreamDownloader _streamDownloader;
		private readonly StreamProcessorFactory _streamProcessorFactory;

		public Application(ILogger logger, StreamDownloader streamDownloader, StreamProcessorFactory fileServiceFactory)
		{
			_logger = logger;
			_streamDownloader = streamDownloader;
			_streamProcessorFactory = fileServiceFactory;
		}

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
