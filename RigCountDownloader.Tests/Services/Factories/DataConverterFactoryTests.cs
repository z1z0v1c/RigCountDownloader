using NSubstitute;
using RigCountDownloader.Domain.Models.Enums;
using RigCountDownloader.Services.DataConverters;
using RigCountDownloader.Services.Factories;
using Xunit;

namespace RigCountDownloader.Tests.Services.Factories
{
	public class DataConverterFactoryTests : TestFixture
	{
		private readonly DataConverterFactory _dataConverterFactory = Substitute.For<DataConverterFactory>();

		[Fact]
		public void CreateDataConverter_ValidMediaType_ReturnsCorrectResult()
		{
			// Arrange
			const string mediaType = MediaType.Spreadsheet;

			// Act
			var xlsxDataConverter = _dataConverterFactory.CreateDataConverter(mediaType);

			// Assert
			Assert.IsType<XlsxDataConverter>(xlsxDataConverter);
		}

		[Fact]
		public void CreateStreamProcessor_InvalidMediaType_ThrowsArgumentException()
		{
			// Arrange
			const string mediaType = "wrong-media.type";

			// Act
			void Act() => _dataConverterFactory.CreateDataConverter(mediaType);

			// Assert
			Assert.Throws<ArgumentException>(Act);
		}
	}
}
