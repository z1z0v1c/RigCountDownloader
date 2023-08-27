using System.Net;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using RichardSzalay.MockHttp;
using Serilog;
using Xunit;

namespace RigCountDownloader.Tests
{
	public class DownloadServiceTests
	{
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly MockHttpMessageHandler _requestHandler;
		private readonly HttpClient _httpClient;
		private readonly StreamDownloader _streamDownloader;

		public DownloadServiceTests()
		{
			this._logger = Substitute.For<ILogger>();
			this._configuration = Substitute.For<IConfiguration>();
			this._requestHandler = new();
			this._httpClient = _requestHandler.ToHttpClient();
			this._streamDownloader = new StreamDownloader(_logger,_configuration, _httpClient);
		}

		[Fact]
		public async Task DownloadFileAsStreamAsync_ValidUri_ReturnsCorrectStream()
		{
			// Arrange
			string inputFileUri = "https://bakerhughesrigcount.gcs-web.com/static-files/7240366e-61cc-4acb-89bf-86dc1a0dffe8";
			_configuration["InputFileUri"].Returns(inputFileUri);

			_requestHandler.When(inputFileUri)
				.Respond(request =>
				{
					return Task.FromResult(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.OK,
						Content = new StreamContent(new MemoryStream(new byte[] { 0x25, 0x50, 0x44, 0x46 }))
					});
				});

			// Act
			Stream file = await _streamDownloader.DownloadFileAsStreamAsync();

			// Assert
			Assert.Equal(4, file.Length);
		}

		[Fact]
		public void DownloadFileAsStreamAsync_InvalidUri_ThrowsArgumentException()
		{
			// Arrange
			string inputFileUri = "https://www.invalidurl.com/nonexisting-file";
			_configuration["InputFileUri"].Returns(inputFileUri);

			_requestHandler.When(inputFileUri)
				.Respond(request =>
				{
					return Task.FromResult(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.NotFound,
					});
				});

			// Act
			async Task act() => await _streamDownloader.DownloadFileAsStreamAsync();

			// Assert
			Assert.ThrowsAsync<ArgumentException>(act);
		}
	}
}
