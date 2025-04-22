using NSubstitute;
using RigCountDownloader.Domain.Models.Enums;
using RigCountDownloader.Services.DataFormatters;
using RigCountDownloader.Services.Factories;
using Xunit;

namespace RigCountDownloader.Tests.Services.Factories
{
    public class DataFormatterFactoryTests : TestFixture
    {
        private readonly DataFormatterFactory _dataFormatterFactory = Substitute.For<DataFormatterFactory>();

        [Fact]
        public void CreateDataFormatter_ValidMediaType_ReturnsCorrectResult()
        {
            // Arrange
            const string mediaType = MediaType.Spreadsheet;

            // Act
            var xlsxDataFormatter = _dataFormatterFactory.CreateDataFormatter(mediaType);

            // Assert
            Assert.IsType<XlsxDataFormatter>(xlsxDataFormatter);
        }

        [Fact]
        public void CreateDataFormatter_InvalidMediaType_ThrowsArgumentException()
        {
            // Arrange
            const string mediaType = "wrong-media.type";

            // Act
            void Act() => _dataFormatterFactory.CreateDataFormatter(mediaType);

            // Assert
            Assert.Throws<ArgumentException>(Act);
        }
    }
}