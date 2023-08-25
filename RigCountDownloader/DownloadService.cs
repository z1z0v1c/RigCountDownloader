using System.Diagnostics;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;

namespace RigCountDownloader
{
	public class DownloadService : IDownloadService
	{
		private readonly IConfiguration _configuration;
		private readonly HttpClient _httpClient;

		public DownloadService(IConfiguration configuration, HttpClient httpClient)
		{
			this._configuration = configuration;
			this._httpClient = httpClient;
			this._httpClient.DefaultRequestHeaders.Add("User-Agent", "RigCountDownloader/1.0");
			this._httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
		}

		public async Task<Stream> DownloadFileAsStreamAsync()
		{
			Uri uri = new(_configuration["BaseAddress"] + _configuration["DownloadPageQuery"]);
			string fileName = _configuration["FileName"];

			HtmlDocument htmlDocument = await GetHtmlDocumentAsync(uri);
			string? fileLink = GetFileLinkFromDocument(htmlDocument, fileName);
			string downloadUri = $"{uri.Scheme}://{uri.Host}:{uri.Port}{fileLink}";

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
				Stream stream = await content.ReadAsStreamAsync();

				return stream;
			}

			return new MemoryStream();
		}

		private async Task<HtmlDocument> GetHtmlDocumentAsync(Uri uri)
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

		private async Task<string> GetHtmlContentAsync(Uri uri)
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

		private static string? GetFileLinkFromDocument(HtmlDocument htmlDocument, string fileName)
		{
			HtmlNode linkNode = htmlDocument.DocumentNode.SelectSingleNode($"//a[@title='{fileName}']");
			return linkNode?.Attributes["href"].Value;
		}
	}
}
