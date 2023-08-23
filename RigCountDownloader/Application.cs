using Microsoft.Extensions.Configuration;

namespace RigCountDownloader
{
	public class Application
	{
		private readonly IConfiguration _configuration;
		private readonly IDownloadService _downloadService;
		private readonly IFileService _fileService;

		public Application(IConfiguration configuration, IDownloadService downloadService, IFileService fileService)
		{
			this._configuration = configuration;
			this._downloadService = downloadService;
			this._fileService = fileService;
		}

		public async Task RunAsync()
		{
			string pageUri = _configuration["BaseAddress"] + _configuration["DownloadPageQuery"];
			string fileName = _configuration["FileName"];

			Stream file = await _downloadService.DownloadFileAsync(pageUri, fileName);

			await _fileService.WriteToFileAsync(file);
		}
	}
}
