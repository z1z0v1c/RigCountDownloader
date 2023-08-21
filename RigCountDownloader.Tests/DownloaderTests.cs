using System.Net;
using HtmlAgilityPack;
using NSubstitute;
using RichardSzalay.MockHttp;
using Xunit;

namespace RigCountDownloader.Tests
{
	public class DownloaderTests
	{
		private readonly MockHttpMessageHandler _handlerSubstitute;
		private readonly HttpClient _httpClient;
		private readonly IDownloader _downloader;

		public DownloaderTests()
		{
			this._handlerSubstitute = new();
			this._httpClient = new(_handlerSubstitute);
			this._downloader = new Downloader(_httpClient);
		}

		[Fact]
		public async Task GetHtmlDocumentAsync_ValidUrl_ReturnsCorrectHtmlDocument()
		{
			// Arrange
			var expectedHtmlContent = @"<!DOCTYPE html>
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

			_handlerSubstitute.When("https://bakerhughesrigcount.gcs-web.com/intl-rig-count?c=79687&p=irol-rigcountsintl")
				.Respond(request =>
			{
				return Task.FromResult(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(expectedHtmlContent)
				});
			});

			// Act
			HtmlDocument htmlDocument = await _downloader.GetHtmlDocumentAsync();

			// Assert
			Assert.Contains("Worldwide Rig Counts - Current &amp; Historical Data", htmlDocument.DocumentNode.InnerHtml);
		}
	}
}
