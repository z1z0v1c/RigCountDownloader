using System.Diagnostics;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;

namespace RigCountDownloader
{
	public class DownloadService : IDownloadService
	{
		private readonly HttpClient _httpClient;
		private readonly IFileService _fileService;


		public DownloadService(HttpClient httpClient, IFileService fileService)
		{
			_httpClient = httpClient;
			_fileService = fileService;
			_httpClient.DefaultRequestHeaders.Add("User-Agent", "RigCountDownloader/1.0");
			_httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
		}

		public async Task<HtmlDocument> GetHtmlDocumentAsync(string uri)
		{
			try
			{
				string htmlContent = await GetHtmlContentAsync(uri);
				return LoadHtml(htmlContent);
			}
			catch (Exception ex)
			{
				Trace.TraceError("An exception occurred: " + ex.Message);
				Trace.TraceError("Stack Trace: " + ex.StackTrace);
				return new HtmlDocument();
			}
		}

		public async Task DownloadFileAsync(HtmlDocument htmlDocument, string baseAddress, string fileName)
		{
			HtmlNode linkNode = htmlDocument.DocumentNode.SelectSingleNode($"//a[@title='{fileName}']");
			string? link = linkNode?.Attributes["href"].Value;
			string downloadUri = baseAddress + link;

			using HttpRequestMessage request = new(HttpMethod.Get, downloadUri);
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

		private async Task<string> GetHtmlContentAsync(string uri)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.GetAsync(uri);
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
