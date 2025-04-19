using NSubstitute;
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
			const string mediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

			// Act
			var xlsxDataConverter = _dataConverterFactory.CreateDataConverter(mediaType);

			// Assert
			Assert.IsType<XlsxDataConverter>(xlsxDataConverter);
		}

		[Fact]
		public void CreateStreamProcessor_InvalidMediaType_ThrowsArgumentException()
		{
			// Arrange
			const string mediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.pdf";

			// Act
			void Act() => _dataConverterFactory.CreateDataConverter(mediaType);

			// Assert
			Assert.Throws<ArgumentException>(Act);
		}
	}
}
