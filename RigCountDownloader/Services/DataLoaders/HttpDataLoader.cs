using RigCountDownloader.Domain.Interfaces;
using RigCountDownloader.Domain.Interfaces.Services;
using RigCountDownloader.Domain.Models;
using Serilog;

namespace RigCountDownloader.Services.DataLoaders
{
	public class HttpDataLoader : IDataLoader
	{
		private readonly ILogger _logger;
		private readonly HttpClient _httpClient;

		public HttpDataLoader(ILogger logger, HttpClient httpClient)
		{
			_logger = logger;
			_httpClient = httpClient;

			ConfigureHttpClient();
		}

		// Configure http client to fully mimic a browser
		private void ConfigureHttpClient()
		{
			_httpClient.DefaultRequestHeaders.Clear();

			_httpClient.DefaultRequestHeaders.Add(
				name: "User-Agent",
				value: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
			);
			_httpClient.DefaultRequestHeaders.Add(
				name: "Accept",
				value: "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7"
			);
			_httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
			_httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
			_httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
			_httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
			_httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Site", "none");
			_httpClient.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
			_httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
			_httpClient.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");

			_httpClient.Timeout = TimeSpan.FromMinutes(5);
		}

		public async Task<Data> LoadDataAsync(Uri uri, CancellationToken cancellationToken = default)
		{
			try
			{
				// Use ResponseHeadersRead to start processing as soon as headers are available
				HttpResponseMessage response = 
					await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

				// Throw an exception if the call is not successful
				response.EnsureSuccessStatusCode();

				string? mediaType = GetMediaType(response);
				string? fileName = GetFileName(response);

				// Create a memory stream that can be returned while allowing the response to be disposed
				var memoryStream = new MemoryStream();

				await using (var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken))
				{
					await contentStream.CopyToAsync(memoryStream, cancellationToken);
				}

				// Reset position to beginning so the caller can read from the start
				memoryStream.Position = 0;

				return new Data(mediaType, fileName, memoryStream);
			}
			catch (HttpRequestException ex)
			{
				_logger.Error(ex, $"HTTP request error when downloading from {uri}");
				if (ex.InnerException != null)
				{
					_logger.Error($"Inner exception: {ex.InnerException.Message}");
				}

				throw;
			}
			catch (TaskCanceledException ex)
			{
				_logger.Error(ex, $"Request timed out when downloading from {uri}");
				throw;
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Unexpected error when downloading from {uri}");
				throw;
			}
		}

		private static string? GetMediaType(HttpResponseMessage response)
		{
			return response.Content.Headers.ContentType?.MediaType;
		}

		private static string? GetFileName(HttpResponseMessage response)
		{
			if (response.Content.Headers.ContentDisposition?.FileNameStar != null)
			{
				return response.Content.Headers.ContentDisposition.FileNameStar;
			}
			else 
			{
				return response.Content.Headers.ContentDisposition?.FileName;
			}
		}
	}
}
