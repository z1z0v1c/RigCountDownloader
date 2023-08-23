using Microsoft.Extensions.Configuration;

namespace RigCountDownloader
{
	public class Application
	{
		private readonly IConfiguration _configuration;
		private readonly IDownloadService _downloadService;

		public Application(IConfiguration configuration, IDownloadService downloadService)
		{
			this._configuration = configuration;
			this._downloadService = downloadService;
		}

		public async Task RunAsync()
		{
			string pageUri = _configuration["BaseAddress"] + _configuration["DownloadPageQuery"];
			string fileName = _configuration["FileName"];

			await _downloadService.DownloadFileAsync(pageUri, fileName);
		}
	}
}
