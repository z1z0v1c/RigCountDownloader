using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using RigCountDownloader.Domain.Interfaces.Services;
using RigCountDownloader.Domain.Interfaces.Services.Factories;
using RigCountDownloader.Domain.Models;
using RigCountDownloader.Services.DataProcessors;
using RigCountDownloader.Services.FileWriters;
using Xunit;

namespace RigCountDownloader.Tests.Services.Factories
{
	public class DataProcessorFactoryTests : TestFixture
	{
		private readonly IDataProcessorFactory _dataProcessorFactory;

		public DataProcessorFactoryTests()
		{
			_dataProcessorFactory = ServiceProvider.GetRequiredService<IDataProcessorFactory>();
		}
		
		[Fact]
		public void CreateXlsxDataProcessor_ValidExtensions_ReturnsCorrectResult()
		{
			// Arrange
			IFileWriter fileWriter = new CsvFileWriter("test1.csv");
			
			const string fileFormat = "xlsx";
			const string fileName = "Worldwide Rig Count.xlsx";
			using var memoryStream = new MemoryStream();

			var data = new ExcelPackage(memoryStream);
			
			// Act
			var dataProcessor = _dataProcessorFactory.CreateDataProcessor(fileWriter, fileFormat, fileName, data);

			// Assert
			Assert.IsType<RigCountDataProcessor>(dataProcessor);
		}

		[Fact]
		public void CreateDataProcessor_InvalidFileType_ThrowsArgumentException()
		{
			// Arrange
			IFileWriter fileWriter = new CsvFileWriter("test2.csv");
			
			const string fileFormat = "pdf";
			const string fileName = "Worldwide Rig Count.xlsx";
			using var memoryStream = new MemoryStream();

			var data = new ExcelPackage(memoryStream);

			// Act
			void Act() => _dataProcessorFactory.CreateDataProcessor(fileWriter, fileFormat, fileName, data);

			// Assert
			Assert.Throws<ArgumentException>(Act);
		}

		[Fact]
		public void CreateDataProcessor_InvalidFileName_ThrowsArgumentException()
		{
			// Arrange
			IFileWriter fileWriter = new CsvFileWriter("test3.csv");
			
			const string fileFormat = "xlsx";
			const string fileName = "Worldwide Rig Rate.xlsx";
			using var memoryStream = new MemoryStream();

			var data = new ExcelPackage(memoryStream);

			// Act
			void Act() => _dataProcessorFactory.CreateDataProcessor(fileWriter, fileFormat, fileName, data);

			// Assert
			Assert.Throws<ArgumentException>(Act);
		}
	}
}
