using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using RigCountDownloader.FileConverters;
using Xunit;

namespace RigCountDownloader.Tests
{
	public class FileConverterFactoryTests : TestFixture
	{
		private readonly IConfiguration _configuration;
		private readonly IFileConverterFactory _fileConverterFactory;

		public FileConverterFactoryTests()
		{
			_configuration = ServiceProvider.GetRequiredService<IConfiguration>();
			_fileConverterFactory = ServiceProvider.GetRequiredService<IFileConverterFactory>();
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
			IFileConverter fileConverter = _fileConverterFactory.CreateFileConverter(response);

			// Assert
			Assert.IsType<WWRCExcelToCsvConverter>(fileConverter);
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
			void act() => _fileConverterFactory.CreateFileConverter(response);

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
			void act() => _fileConverterFactory.CreateFileConverter(response);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}
	}
}
