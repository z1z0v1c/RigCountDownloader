using System.Net;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using RichardSzalay.MockHttp;
using Xunit;

namespace RigCountDownloader.Tests
{
	public class DownloadServiceTests
	{
		private readonly IConfiguration _configuration;
		private readonly MockHttpMessageHandler _requestHandler;
		private readonly HttpClient _httpClient;
		private readonly IDownloadService _downloader;

		public DownloadServiceTests()
		{
			this._configuration = Substitute.For<IConfiguration>();
			this._requestHandler = new();
			this._httpClient = _requestHandler.ToHttpClient();
			this._downloader = new DownloadService(_configuration, _httpClient);
		}

		[Fact]
		public async Task DownloadFileAsync_ValidUri_ReturnsCorrectStream()
		{
			// Arrange
			string url = "https://bakerhughesrigcount.gcs-web.com";
			string fileLink = "/static-files/7240366e-61cc-4acb-89bf-86dc1a0dffe8";

			_configuration["BaseAddress"].Returns(url);
			_configuration["FileLink"].Returns(fileLink);

			_requestHandler.When(url + fileLink)
				.Respond(request =>
				{
					return Task.FromResult(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.OK,
						Content = new StreamContent(new MemoryStream(new byte[] { 0x25, 0x50, 0x44, 0x46 }))
					});
				});

			// Act
			Stream file = await _downloader.DownloadFileAsStreamAsync();

			// Assert
			Assert.Equal(4, file.Length);
		}

		[Fact]
		public async Task DownloadFileAsync_InvalidUri_ReturnsNullMemoryStream()
		{
			// Arrange
			string url = "https://www.invalidurl.com";
			string fileLink = "/nonexisting-file";

			_configuration["BaseAddress"].Returns(url);
			_configuration["FileLink"].Returns(fileLink);

			_requestHandler.When(url + fileLink)
				.Respond(request =>
				{
					return Task.FromResult(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.BadRequest,
						Content = new StreamContent(MemoryStream.Null)
					});
				});

			// Act
			Stream file = await _downloader.DownloadFileAsStreamAsync();

			// Assert
			Assert.Equal(MemoryStream.Null, file);
		}
	}
}
