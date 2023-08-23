using System.Diagnostics;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;

namespace RigCountDownloader
{
	public class DownloadService : IDownloadService
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly IFileService _fileService;



		public DownloadService(HttpClient httpClient, IConfiguration configuration, IFileService fileService)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_fileService = fileService;
			_httpClient.DefaultRequestHeaders.Add("User-Agent", "RigCountDownloader/1.0");
			_httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
		}

		public async Task DownloadFileAsync(HtmlDocument htmlDocument)
		{
			HtmlNode linkNode = htmlDocument.DocumentNode.SelectSingleNode($"//a[@title='{_configuration["FileName"]}']");
			var link = linkNode?.Attributes["href"].Value;

			using var request = new HttpRequestMessage(HttpMethod.Get, _configuration["BaseAddress"] + link);
			HttpResponseMessage response = new();

			try
			{
				response = await _httpClient.SendAsync(request);
			}
			catch (Exception ex)
			{
				Console.WriteLine("An exception occurred: " + ex.Message);
				Console.WriteLine("Stack Trace: " + ex.StackTrace);
			}

			if (response.IsSuccessStatusCode)
			{
				HttpContent content = response.Content;
				await _fileService.WriteToFileAsync(content);

				Console.WriteLine($"File downloaded successfully.");
			}
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
				HttpResponseMessage response = await _httpClient.GetAsync(_configuration["BaseAddress"] + _configuration["DownloadPageQuery"]);
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

		private static HtmlDocument LoadHtml(string htmlContent)
		{
			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(htmlContent);
			return htmlDocument;
		}
	}
}
