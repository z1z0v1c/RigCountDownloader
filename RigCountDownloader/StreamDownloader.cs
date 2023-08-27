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
			this._logger = logger;
			this._configuration = configuration;
			this._httpClient = httpClient;
			this._httpClient.DefaultRequestHeaders.Add("User-Agent", "RigCountDownloader/1.0");
			this._httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
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
