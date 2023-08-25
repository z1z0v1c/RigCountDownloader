using Microsoft.Extensions.Configuration;

namespace RigCountDownloader
{
	public class StreamDownloader
	{
		private readonly IConfiguration _configuration;
		private readonly HttpClient _httpClient;

		public StreamDownloader(IConfiguration configuration, HttpClient httpClient)
		{
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

			return MemoryStream.Null;
		}
	}
}
