using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using RigCountDownloader.FileConverters;
using Xunit;

namespace RigCountDownloader.Tests
{
	public class DataProcessorFactoryTests : TestFixture
	{
		private readonly IConfiguration _configuration;
		private readonly IDataProcessorFactory _dataProcessorFactory;

		public DataProcessorFactoryTests()
		{
			_configuration = ServiceProvider.GetRequiredService<IConfiguration>();
			_dataProcessorFactory = ServiceProvider.GetRequiredService<IDataProcessorFactory>();
		}

		[Fact]
		public void CreateFileConverter_VallidExtensions_ReturnsCorrectFileConverter()
		{
			// Arrange
			string mediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			string fileName = "Worldwide Rig Count.xlsx";
			using MemoryStream memoryStream = new();

			Response response = new(mediaType, fileName, memoryStream);

			_configuration["OutputFileType"].Returns("csv");

			// Act
			IDataProcessor dataProcessor = _dataProcessorFactory.CreateFileConverter(response);

			// Assert
			Assert.IsType<RigCountDataProcessor>(dataProcessor);
		}

		[Fact]
		public void CreateStreamProcessor_InvallidInputExtension_ThrowsArgumentException()
		{
			// Arrange
			string mediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.pdf";
			string fileName = "Worldwide Rig Count.xlsx";
			using MemoryStream memoryStream = new();

			Response response = new(mediaType, fileName, memoryStream);

			_configuration["OutputFileLocation"].Returns("Worldwide Rig Count Jul 2023.csv");

			// Act
			void act() => _dataProcessorFactory.CreateFileConverter(response);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}

		[Fact]
		public void CreateStreamProcessor_InvallidOutputExtension_ThrowsArgumentException()
		{
			// Arrange
			string mediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.pdf";
			string fileName = "Worldwide Rig Count.xlsx";
			using MemoryStream memoryStream = new();

			Response response = new(mediaType, fileName, memoryStream);

			_configuration["OutputFileName"].Returns("Worldwide Rig Count Jul 2023.docx");

			// Act
			void act() => _dataProcessorFactory.CreateFileConverter(response);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}
	}
}
