using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using RigCountDownloader.FileConverters;
using RigCountDownloader.StreamProcessors;
using Serilog;
using Xunit;

namespace RigCountDownloader.Tests
{
	public class StreamProcessorFactoryTests : TestFixture
	{
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly IFileConverterFactory _fileConverterFactory;
		private readonly StreamProcessorFactory _streamProcessorFactory;

		public StreamProcessorFactoryTests()
		{
			_logger = ServiceProvider.GetRequiredService<ILogger>();
			_configuration = ServiceProvider.GetRequiredService<IConfiguration>();
			_fileConverterFactory = Substitute.For<IFileConverterFactory>();
			_streamProcessorFactory = new(_configuration, _fileConverterFactory);
		}

		[Fact]
		public void CreateStreamProcessor_VallidExtension_ReturnsCorrectStreamProcessor()
		{
			// Arrange
			string mediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			string fileName = "Worldwide Rig Count.xlsx";
			using MemoryStream memoryStream = new();

			Response response = new(mediaType, fileName, memoryStream);

			_fileConverterFactory.CreateFileConverter(response).Returns(new WWRCExcelToCsvConverter(_logger, _configuration));

			// Act
			IStreamProcessor excelStreamProcessor = _streamProcessorFactory.CreateStreamProcessor(response);

			// Assert
			Assert.IsType<ExcelStreamProcessor>(excelStreamProcessor);
		}

		[Fact]
		public void CreateStreamProcessor_InvallidExtension_ThrowsArgumentException()
		{
			// Arrange
			string mediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.pdf";
			string fileName = "Worldwide Rig Count.xlsx";
			using MemoryStream memoryStream = new();

			Response response = new(mediaType, fileName, memoryStream);

			// Act
			void act() => _streamProcessorFactory.CreateStreamProcessor(response);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}
	}
}
