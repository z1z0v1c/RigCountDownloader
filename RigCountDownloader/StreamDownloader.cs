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
			Uri uri = new(_configuration["InputFileUri"] ?? string.Empty);

			using HttpRequestMessage request = new(HttpMethod.Get, uri);

			_logger.Information($"Downloading file from {uri}...");

			HttpResponseMessage response = await _httpClient.SendAsync(request);

			if (response.IsSuccessStatusCode)
			{
				HttpContent content = response.Content;
				Stream stream = await content.ReadAsStreamAsync();

				_logger.Information($"Download completed successfuly.");

				return stream;
			}

			throw new ArgumentException($"File from {uri} {response.ReasonPhrase?.ToLower()}. Check appsettings.json file.");
		}
	}
}
