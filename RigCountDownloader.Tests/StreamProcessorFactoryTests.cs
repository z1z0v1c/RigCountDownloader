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
			this._logger = ServiceProvider.GetRequiredService<ILogger>();
			this._configuration = ServiceProvider.GetRequiredService<IConfiguration>();
			this._fileConverterFactory = Substitute.For<IFileConverterFactory>();
			this._streamProcessorFactory = new(_configuration, _fileConverterFactory);
		}

		[Fact]
		public void CreateStreamProcessor_VallidExtension_ReturnsCorrectStreamProcessor()
		{
			// Arrange
			_configuration["InputFileName"].Returns("Worldwide Rig Count Jul 2023.xlsx");
			_fileConverterFactory.CreateFileConverter().Returns(new WWRCExcelToCsvConverter(_logger, _configuration));

			// Act
			IStreamProcessor excelStreamProcessor = _streamProcessorFactory.CreateStreamProcessor();

			// Assert
			Assert.IsType<ExcelStreamProcessor>(excelStreamProcessor);
		}

		[Fact]
		public void CreateStreamProcessor_InvallidExtension_ThrowsArgumentException()
		{
			// Arrange
			_configuration["InputFileName"].Returns("Worldwide Rig Count Jul 2023.pdf");

			// Act
			void act() => _streamProcessorFactory.CreateStreamProcessor();

			// Assert
			Assert.Throws<ArgumentException>(act);
		}
	}
}
