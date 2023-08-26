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
			Uri uri = new(_configuration["InputFileUri"]);

			using HttpRequestMessage request = new(HttpMethod.Get, uri);
			HttpResponseMessage response = new();

			_logger.Information($"Downloading file from {uri}...");
			try
			{
				response = await _httpClient.SendAsync(request);
			}
			catch (Exception ex)
			{
				_logger.Error("An exception occurred: " + ex.Message);
				_logger.Error("Stack Trace: " + ex.StackTrace);
			}

			if (response.IsSuccessStatusCode)
			{
				HttpContent content = response.Content;
				Stream stream = await content.ReadAsStreamAsync();

				_logger.Information($"Download completed successfuly.");

				return stream;
			}

			// Throw an exception?
			return MemoryStream.Null;
		}
	}
}
