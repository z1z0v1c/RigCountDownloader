using System.Net;
using NSubstitute;
using RichardSzalay.MockHttp;
using Xunit;

namespace RigCountDownloader.Tests
{
	public class DownloadServiceTests
	{
		private readonly MockHttpMessageHandler _requestHandler;
		private readonly HttpClient _httpClient;
		private readonly IDownloadService _downloader;

		public DownloadServiceTests()
		{
			this._requestHandler = new();
			this._httpClient = _requestHandler.ToHttpClient();
			this._downloader = new DownloadService(_httpClient);
		}

		[Fact]
		public async Task DownloadFileAsync_ValidUri_ReturnsCorrectStream()
		{
			// Arrange
			string url = "https://bakerhughesrigcount.gcs-web.com";
			string query = "/intl-rig-count?c=79687&p=irol-rigcountsintl";
			string fileName = "Worldwide Rig Count Jul 2023.xlsx";
			string fileLink = "/static-files/7240366e-61cc-4acb-89bf-86dc1a0dffe8";

			string expectedHtmlContent = @"<!DOCTYPE html>
										<html>
											<body>
												<a href=""/static-files/7240366e-61cc-4acb-89bf-86dc1a0dffe8"" 
												type=""application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"" 
												title=""Worldwide Rig Count Jul 2023.xlsx"" 
												target=""_blank"">
													Worldwide Rig Counts - Current &amp; Historical Data
												</a>
											</body>
										</html>";

			_requestHandler.When(url + query)
				.Respond(request =>
				{
					return Task.FromResult(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.OK,
						Content = new StringContent(expectedHtmlContent)
					});
				});

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
			Stream file = await _downloader.DownloadFileAsync(url + query, fileName);

			// Assert
			Assert.Equal(4, file.Length);
		}

		[Fact]
		public async Task DownloadFileAsync_InvalidUri_ReturnsEmptyMemoryStream()
		{
			// Arrange
			string uri = "https://www.invalidurl.com";
			string fileName = "Nonexisting file.xlsx";
			string fileLink = "/no-file";

			_requestHandler.When(uri)
				.Respond(request =>
				{
					return Task.FromResult(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.OK,
						Content = new StringContent("<a href=\"/no-file\" title=\"Nonexisting file.xlsx\"</a>")
					});
				});

			_requestHandler.When(uri + fileLink)
				.Respond(request =>
				{
					return Task.FromResult(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.OK,
						Content = new StreamContent(new MemoryStream())
					});
				});

			// Act
			Stream file = await _downloader.DownloadFileAsync(uri, fileName);

			// Assert
			Assert.Equal(0, file.Length);
		}
	}
}
