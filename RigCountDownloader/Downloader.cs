using System.Diagnostics;
using HtmlAgilityPack;

namespace RigCountDownloader
{
	public class Downloader : IDownloader
	{
		private static readonly string Url = "https://bakerhughesrigcount.gcs-web.com/intl-rig-count?c=79687&p=irol-rigcountsintl";

		private readonly HttpClient _httpClient;

		public Downloader(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.DefaultRequestHeaders.Add("User-Agent", "RigCountDownloader/1.0");
			_httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
			_httpClient.BaseAddress = new Uri(Url);
		}

		public async Task<HtmlDocument> GetHtmlDocumentAsync()
		{
			try
			{
				string htmlContent = await GetHtmlContentAsync();
				return LoadHtml(htmlContent);
			}
			catch (Exception ex)
			{
				Trace.TraceError("An exception occurred: " + ex.Message);
				Trace.TraceError("Stack Trace: " + ex.StackTrace);
				return new HtmlDocument();
			}
		}

		private async Task<string> GetHtmlContentAsync()
		{
			try
			{
				HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress);
				if (response.IsSuccessStatusCode)
				{
					return await response.Content.ReadAsStringAsync();
				}
				else
				{
					Console.WriteLine($"Failed to get content. Status code: {response.StatusCode}");
					return string.Empty;
				}
			}
			catch (Exception ex)
			{
				Trace.TraceError("An exception occurred: " + ex.Message);
				Trace.TraceError("Stack Trace: " + ex.StackTrace);
				return string.Empty;
			}
		}

		private HtmlDocument LoadHtml(string htmlContent)
		{
			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(htmlContent);
			return htmlDocument;
		}
	}
}
