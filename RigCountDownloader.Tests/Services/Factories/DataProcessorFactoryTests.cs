using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using RigCountDownloader.Domain.Interfaces.Factories;
using RigCountDownloader.Domain.Models;
using RigCountDownloader.Services.DataProcessors;
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
			const string fileType = "xlsx";
			const string fileName = "Worldwide Rig Count.xlsx";
			using var memoryStream = new MemoryStream();

			var data = new XlsxData(fileType, fileName, new ExcelPackage(memoryStream));

			// Act
			var dataProcessor = _dataProcessorFactory.CreateDataProcessor(data);

			// Assert
			Assert.IsType<RigCountDataProcessor>(dataProcessor);
		}

		[Fact]
		public void CreateDataProcessor_InvalidFileType_ThrowsArgumentException()
		{
			// Arrange
			const string fileType = "pdf";
			const string fileName = "Worldwide Rig Count.xlsx";
			using var memoryStream = new MemoryStream();

			var data = new XlsxData(fileType, fileName, memoryStream);

			// Act
			void Act() => _dataProcessorFactory.CreateDataProcessor(data);

			// Assert
			Assert.Throws<ArgumentException>(Act);
		}

		[Fact]
		public void CreateDataProcessor_InvalidFileName_ThrowsArgumentException()
		{
			// Arrange
			const string fileType = "xlsx";
			const string fileName = "Worldwide Rig Rate.xlsx";
			using var memoryStream = new MemoryStream();

			var data = new XlsxData(fileType, fileName, memoryStream);

			// Act
			void Act() => _dataProcessorFactory.CreateDataProcessor(data);

			// Assert
			Assert.Throws<ArgumentException>(Act);
		}
	}
}
