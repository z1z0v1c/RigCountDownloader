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
			this._configuration = ServiceProvider.GetRequiredService<IConfiguration>();
			this._fileConverterFactory = ServiceProvider.GetRequiredService<IFileConverterFactory>();
		}

		[Fact]
		public void CreateFileConverter_VallidExtensions_ReturnsCorrectFileConverter()
		{
			// Arrange
			_configuration["InputFileName"].Returns("Worldwide Rig Count Jul 2023.xlsx");
			_configuration["OutputFileName"].Returns("Worldwide Rig Count Jul 2023.csv");

			// Act
			IFileConverter fileConverter = _fileConverterFactory.CreateFileConverter();

			// Assert
			Assert.IsType<WWRCExcelToCsvConverter>(fileConverter);
		}

		[Fact]
		public void CreateStreamProcessor_InvallidInputExtension_ThrowsArgumentException()
		{
			// Arrange
			_configuration["InputFileName"].Returns("Worldwide Rig Count Jul 2023.pdf");
			_configuration["OutputFileName"].Returns("Worldwide Rig Count Jul 2023.csv");

			// Act
			void act() => _fileConverterFactory.CreateFileConverter();

			// Assert
			Assert.Throws<ArgumentException>(act);
		}

		[Fact]
		public void CreateStreamProcessor_InvallidOutputExtension_ThrowsArgumentException()
		{
			// Arrange
			_configuration["InputFileName"].Returns("Worldwide Rig Count Jul 2023.xlsx");
			_configuration["OutputFileName"].Returns("Worldwide Rig Count Jul 2023.docx");

			// Act
			void act() => _fileConverterFactory.CreateFileConverter();

			// Assert
			Assert.Throws<ArgumentException>(act);
		}
	}
}
