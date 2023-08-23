using HtmlAgilityPack;
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
			HtmlDocument htmlDocument = await _downloadService.GetHtmlDocumentAsync(_configuration["BaseAddress"] + _configuration["DownloadPageQuery"]);

			await _downloadService.DownloadFileAsync(htmlDocument, _configuration["BaseAddress"], _configuration["FileName"]);
		}
	}
}
