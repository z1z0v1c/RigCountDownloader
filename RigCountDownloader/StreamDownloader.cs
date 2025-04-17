using Microsoft.Extensions.Configuration;
using Serilog;

namespace RigCountDownloader
{
	public class StreamDownloader
	{
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly HttpClient _httpClient;

		public StreamDownloader(ILogger logger, IConfiguration configuration, HttpClient httpClient)
		{
			_logger = logger;
			_configuration = configuration;
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

		public async Task<Stream> DownloadFileAsStreamAsync()
		{
			string uriString = _configuration["InputFileUri"] ?? string.Empty;
			if (string.IsNullOrEmpty(uriString))
			{
				throw new ArgumentException("InputFileUri configuration value is missing or empty.");
			}

			Uri uri = new(uriString);
			_logger.Information($"Downloading file from {uri}...");

			try
			{
				// Use ResponseHeadersRead to start processing as soon as headers are available
				HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
				// Throw an exception if the call is not successful
				response.EnsureSuccessStatusCode();

				// Create a memory stream that can be returned while allowing the response to be disposed
				var memoryStream = new MemoryStream();
				await using (var contentStream = await response.Content.ReadAsStreamAsync())
				{
					await contentStream.CopyToAsync(memoryStream);
				}

				// Reset position to beginning so the caller can read from the start
				memoryStream.Position = 0;
				_logger.Information($"Download completed successfully. Received {memoryStream.Length} bytes.");

				return memoryStream;
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
	}
}
