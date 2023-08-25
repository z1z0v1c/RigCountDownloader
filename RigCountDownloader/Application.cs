using RigCountDownloader.StreamProcessors;

namespace RigCountDownloader
{
    public class Application
	{
		private readonly StreamDownloader _streamDownloader;
		private readonly StreamProcessorFactory _streamProcessorFactory;

		public Application(StreamDownloader streamDownloader, StreamProcessorFactory fileServiceFactory)
		{
			this._streamDownloader = streamDownloader;
			this._streamProcessorFactory = fileServiceFactory;
		}

		public async Task RunAsync()
		{
			Stream fileStream = await _streamDownloader.DownloadFileAsStreamAsync();

			IStreamProcessor streamProcessor = _streamProcessorFactory.CreateStreamProcessor();
			await streamProcessor.ProcessStreamAsync(fileStream);
		}
	}
}
