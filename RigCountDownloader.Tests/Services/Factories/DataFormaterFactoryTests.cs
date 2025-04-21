using NSubstitute;
using RigCountDownloader.Domain.Models.Enums;
using RigCountDownloader.Services.DataConverters;
using RigCountDownloader.Services.Factories;
using Xunit;

namespace RigCountDownloader.Tests.Services.Factories
{
	public class DataFormaterFactoryTests : TestFixture
	{
		private readonly DataFormaterFactory _dataFormaterFactory = Substitute.For<DataFormaterFactory>();

		[Fact]
		public void CreateDataConverter_ValidMediaType_ReturnsCorrectResult()
		{
			// Arrange
			const string mediaType = MediaType.Spreadsheet;

			// Act
			var xlsxDataConverter = _dataFormaterFactory.CreateDataFormater(mediaType);

			// Assert
			Assert.IsType<XlsxDataFormater>(xlsxDataConverter);
		}

		[Fact]
		public void CreateStreamProcessor_InvalidMediaType_ThrowsArgumentException()
		{
			// Arrange
			const string mediaType = "wrong-media.type";

			// Act
			void Act() => _dataFormaterFactory.CreateDataFormater(mediaType);

			// Assert
			Assert.Throws<ArgumentException>(Act);
		}
	}
}
