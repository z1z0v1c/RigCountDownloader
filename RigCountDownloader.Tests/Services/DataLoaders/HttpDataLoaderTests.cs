using System.Net;

namespace RigCountDownloader.Tests.Services.DataLoaders
{
    public class HttpDataLoaderTests : TestFixture
    {
        private readonly HttpDataLoader _httpDataLoader;
        private readonly MockHttpMessageHandler _requestHandler;

        public HttpDataLoaderTests()
        {
            var logger = ServiceProvider.GetRequiredService<ILogger>();
            _requestHandler = ServiceProvider.GetRequiredService<MockHttpMessageHandler>();
            _httpDataLoader = new HttpDataLoader(logger, _requestHandler.ToHttpClient());
        }

        [Fact]
        public async Task LoadDataAsync_ValidSourceFileLocation_ReturnsCorrectData()
        {
            // Arrange
            const string sourceFileLocation = "https://validurl.com/existing-file";

            var memoryStreamBytes = new byte[]
            {
                0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00, 0x08, 0x00, 0x00, 0x00, 0x21, 0x00, 0x9C, 0x8E
            };

            _requestHandler.When(sourceFileLocation)
                .Respond(request => Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StreamContent(new MemoryStream(memoryStreamBytes))
                }));

            // Act
            DataStream dataStream = await _httpDataLoader.LoadDataAsync(sourceFileLocation);

            // Assert
            Assert.Equal(memoryStreamBytes.Length, dataStream.MemoryStream.Length);
        }

        [Fact]
        public async Task LoadDataAsync_InvalidSourceFileLocation_ThrowsHttpDataLoadException()
        {
            // Arrange
            const string sourceFileLocation = "https://invalidurl.com/nonexisting-file";

            _requestHandler.When(sourceFileLocation)
                .Respond(request => Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                }));

            // Act
            async Task Act() => await _httpDataLoader.LoadDataAsync(sourceFileLocation);

            // Assert
            await Assert.ThrowsAsync<HttpDataLoadException>(Act);
        }

        [Fact]
        public async Task LoadDataAsync_NullSourceFileLocation_ThrowsIncorrectSettingsException()
        {
            // Arrange
            string? sourceFileLocation = null;

            // Act
            async Task Act() => await _httpDataLoader.LoadDataAsync(sourceFileLocation!);

            // Assert
            await Assert.ThrowsAsync<IncorrectSettingsException>(Act);
        }
    }
}