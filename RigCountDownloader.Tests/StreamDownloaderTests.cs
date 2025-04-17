using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using RichardSzalay.MockHttp;
using Serilog;
using Xunit;

namespace RigCountDownloader.Tests
{
	public class StreamDownloaderTests : TestFixture
	{
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly MockHttpMessageHandler _requestHandler;
		private readonly StreamDownloader _streamDownloader;

		public StreamDownloaderTests()
		{
			_logger = ServiceProvider.GetRequiredService<ILogger>();
			_configuration = ServiceProvider.GetRequiredService<IConfiguration>();
			_requestHandler = ServiceProvider.GetRequiredService<MockHttpMessageHandler>();
			_streamDownloader = new StreamDownloader(_logger, _configuration, _requestHandler.ToHttpClient());
		}

		[Fact]
		public async Task DownloadFileAsStreamAsync_ValidUri_ReturnsCorrectStream()
		{
			// Arrange
			string inputFileUri = "https://validurl.com/existing-file";
			_configuration["InputFileUri"].Returns(inputFileUri);

			var memoryStreamBytes = new byte[]
			{
				0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00, 0x08, 0x00, 0x00, 0x00, 0x21, 0x00, 0x9C, 0x8E
			};

			_requestHandler.When(inputFileUri)
				.Respond(request =>
				{
					return Task.FromResult(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.OK,
						Content = new StreamContent(new MemoryStream(memoryStreamBytes))
					});
				});

			// Act
			Stream file = await _streamDownloader.DownloadFileAsStreamAsync();

			// Assert
			Assert.Equal(memoryStreamBytes.Length, file.Length);
		}

		[Fact]
		public async Task DownloadFileAsStreamAsync_InvalidUri_ThrowsArgumentException()
		{
			// Arrange
			string inputFileUri = "https://invalidurl.com/nonexisting-file";
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
            await Assert.ThrowsAsync<ArgumentException>(act);
		}

		[Fact]
		public async Task DownloadFileAsStreamAsync_NoUriProvided_ThrowsUriFormatException()
		{
			// Arrange
			string? inputFileUri = null;
			_configuration["InputFileUri"].Returns(inputFileUri);

			// Act
			async Task act() => await _streamDownloader.DownloadFileAsStreamAsync();

            // Assert
            await Assert.ThrowsAsync<UriFormatException>(act);
		}
	}
}
