using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RigCountDownloader
{
	internal class Application
	{
		private readonly IDownloadService _downloadService;

		public Application(IDownloadService downloadService)
		{
			this._downloadService = downloadService;
		}

		public async Task RunAsync()
		{
			HtmlDocument htmlDocument = await _downloadService.GetHtmlDocumentAsync();

			await _downloadService.DownloadFileAsync(htmlDocument);
		}
	}
}
