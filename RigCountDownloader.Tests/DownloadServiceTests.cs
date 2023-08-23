using System.Net;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using RichardSzalay.MockHttp;
using Xunit;

namespace RigCountDownloader.Tests
{
	public class DownloadServiceTests
	{
		private readonly MockHttpMessageHandler _requestHandler;
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly IDownloadService _downloader;

		public DownloadServiceTests()
		{
			this._requestHandler = new();
			this._httpClient = new(_requestHandler);
			this._configuration = Substitute.For<IConfiguration>();
			this._downloader = new DownloadService(_httpClient, _configuration, new ExcelFileService(_configuration));
		}

		[Fact]
		public async Task GetHtmlDocumentAsync_ValidUrl_ReturnsCorrectHtmlDocument()
		{
			// Arrange
			string baseAddress = "https://bakerhughesrigcount.gcs-web.com";
			string downloadPageQuery = "/intl-rig-count?c=79687&p=irol-rigcountsintl";

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

			_configuration["BaseAddress"].Returns(baseAddress);
			_configuration["DownloadPageQuery"].Returns(downloadPageQuery);

			_requestHandler.When(baseAddress + downloadPageQuery)
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

		[Fact]
		public async Task GetHtmlDocumentAsync_InvalidUrl_ReturnsEmptyHtmlDocument()
		{
			// Arrange
			string baseAddress = "https://www.invalidurl.com";
			string downloadPageQuery = string.Empty;
			string expectedHtmlContent = string.Empty;

			_configuration["BaseAddress"].Returns(baseAddress);
			_configuration["DownloadPageQuery"].Returns(downloadPageQuery);

			_requestHandler.When(baseAddress + downloadPageQuery)
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
			Assert.Equal(string.Empty, htmlDocument.DocumentNode.InnerHtml);
		}
	}
}
