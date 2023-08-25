namespace RigCountDownloader
{
	public class Application
	{
		private readonly IDownloadService _downloadService;
		private readonly FileServiceFactory _fileServiceFactory;

		public Application(IDownloadService downloadService, FileServiceFactory fileServiceFactory)
		{
			this._downloadService = downloadService;
			this._fileServiceFactory = fileServiceFactory;
		}

		public async Task RunAsync()
		{
			Stream file = await _downloadService.DownloadFileAsStreamAsync();

			IFileService fileService = _fileServiceFactory.CreateFileService();
			await fileService.WriteToFileAsync(file);
		}
	}
}
