using System.Net;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;
using RigCountDownloader.Domain.Models;
using RigCountDownloader.Services.DataLoaders;
using Serilog;
using Xunit;

namespace RigCountDownloader.Tests.Services.DataLoaders
{
	public class HttpDataLoaderTests : TestFixture
	{
		private readonly MockHttpMessageHandler _requestHandler;
		private readonly HttpDataLoader _httpDataLoader;

		public HttpDataLoaderTests()
		{
			var logger = ServiceProvider.GetRequiredService<ILogger>();
			_requestHandler = ServiceProvider.GetRequiredService<MockHttpMessageHandler>();
			_httpDataLoader = new HttpDataLoader(logger, _requestHandler.ToHttpClient());
		}

		[Fact]
		public async Task LoadDataAsync_ValidUri_ReturnsCorrectData()
		{
			// Arrange
			const string inputFileUri = "https://validurl.com/existing-file";

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
			Data data = await _httpDataLoader.LoadDataAsync(new Uri(inputFileUri));

			// Assert
			Assert.Equal(memoryStreamBytes.Length, data.MemoryStream.Length);
		}

		[Fact]
		public async Task DownloadFileAsStreamAsync_InvalidUri_ThrowsHttpRequestException()
		{
			// Arrange
			const string inputFileUri = "https://invalidurl.com/nonexisting-file";

			_requestHandler.When(inputFileUri)
				.Respond(request =>
				{
					return Task.FromResult(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.NotFound,
					});
				});

			// Act
			async Task act() => await _httpDataLoader.LoadDataAsync(new Uri(inputFileUri));

            // Assert
            await Assert.ThrowsAsync<HttpRequestException>(act);
		}

		[Fact]
		public async Task DownloadFileAsStreamAsync_NoUriProvided_ThrowsUriFormatException()
		{
			// Arrange
			string? inputFileUri = null;

			// Act
			async Task act() => await _httpDataLoader.LoadDataAsync(new Uri(inputFileUri!));

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(act);
		}
	}
}
