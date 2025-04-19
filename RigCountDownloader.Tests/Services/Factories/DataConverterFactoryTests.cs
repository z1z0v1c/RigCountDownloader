using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using RigCountDownloader.FileConverters;
using RigCountDownloader.Services.Factories;
using RigCountDownloader.StreamProcessors;
using Serilog;
using Xunit;

namespace RigCountDownloader.Tests
{
	public class DataConverterFactoryTests : TestFixture
	{
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly IDataProcessorFactory _dataProcessorFactory;
		private readonly DataConverterFactory _dataConverterFactory;

		public DataConverterFactoryTests()
		{
			_logger = ServiceProvider.GetRequiredService<ILogger>();
			_configuration = ServiceProvider.GetRequiredService<IConfiguration>();
			_dataProcessorFactory = Substitute.For<IDataProcessorFactory>();
			_dataConverterFactory = new(_dataProcessorFactory);
		}

		[Fact]
		public void CreateStreamProcessor_VallidExtension_ReturnsCorrectStreamProcessor()
		{
			// Arrange
			string mediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			string fileName = "Worldwide Rig Count.xlsx";
			using MemoryStream memoryStream = new();

			Data data = new(mediaType, fileName, memoryStream);

			_dataProcessorFactory.CreateFileConverter(data).Returns(new RigCountDataProcessor(_logger, _configuration));

			// Act
			IDataConverter excelDataConverter = _dataConverterFactory.CreateDataConverter(data);

			// Assert
			Assert.IsType<XlsxDataConverter>(excelDataConverter);
		}

		[Fact]
		public void CreateStreamProcessor_InvallidExtension_ThrowsArgumentException()
		{
			// Arrange
			string mediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.pdf";
			string fileName = "Worldwide Rig Count.xlsx";
			using MemoryStream memoryStream = new();

			Data data = new(mediaType, fileName, memoryStream);

			// Act
			void act() => _dataConverterFactory.CreateDataConverter(data);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}
	}
}
