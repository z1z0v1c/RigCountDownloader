using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using OfficeOpenXml;
using RigCountDownloader.Services.DataProcessors;
using Serilog;
using Xunit;

namespace RigCountDownloader.Tests.Services.DataProcessors
{
	public class RigCountDataProcessorTests : TestFixture
	{
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly RigCountDataProcessor _dataProcessor;

		public RigCountDataProcessorTests()
		{
			_logger = ServiceProvider.GetRequiredService<ILogger>();
			_configuration = ServiceProvider.GetRequiredService<IConfiguration>();
			_dataProcessor = new RigCountDataProcessor(_logger, _configuration, new ExcelPackage());
		}

		[Fact]
		public async Task ProcessAndSaveAsync_ValidData_WritesToCsvFile()
		{
			// Arrange
			const string fileName = "Worldwide Rig Count Jul 2013.csv";
			var outputFilePath = $"{Directory.GetCurrentDirectory()}\\{fileName}";
			_configuration["OutputFileLocation"].Returns(fileName);

			var worksheet = _dataProcessor.ExcelPackage.Workbook.Worksheets.Add("Sheet");
			worksheet.Cells[1, 1].Value = "Europe";
			worksheet.Cells[2, 1].Value = "Avg.";
			worksheet.Cells[3, 1].Value = "Avg.";

			// Act
			await _dataProcessor.ProcessAndSaveAsync();

			// Assert
			_logger.Received().Information($"The CSV file saved to {outputFilePath}.");
		}

		[Fact]
		public async Task ProcessAndSaveAsync_InvalidData_ThrowsInvalidDataException()
		{
			// Arrange
			const string fileName = "Worldwide Rig Count Jul 2014.csv";
			_configuration["OutputFileLocation"].Returns(fileName);

			var worksheet = _dataProcessor.ExcelPackage.Workbook.Worksheets.Add("Sheet");
			worksheet.Cells[1, 1].Value = "Europe";
			worksheet.Cells[2, 1].Value = "Europe";
			worksheet.Cells[3, 1].Value = "Avg.";

			// Act
			async Task Act() => await _dataProcessor.ProcessAndSaveAsync();

			// Assert
			await Assert.ThrowsAsync<InvalidDataException>(Act);
		}

		[Fact]
		public async Task ProcessAndSaveAsync_EmptyWorksheet_ThrowsInvalidDataException()
		{
			// Arrange
			const string fileName = "Worldwide Rig Count Jul 2015.csv";
			_configuration["OutputFileLocation"].Returns(fileName);

			_dataProcessor.ExcelPackage.Workbook.Worksheets.Add("EmptyWorksheet");
			
			// Act
			async Task Act() => await _dataProcessor.ProcessAndSaveAsync();

			// Assert
			await Assert.ThrowsAsync<InvalidDataException>(Act);
		}

		[Fact]
		public async Task  ProcessAndSaveAsync_NoData_ThrowsInvalidDataException()
		{
			// Arrange
			const string fileName = "Worldwide Rig Count Jul 2016.csv";
			_configuration["OutputFileLocation"].Returns(fileName);

			using var memoryStream = new MemoryStream();
			using var package = new ExcelPackage();

			_dataProcessor.ExcelPackage = package;
			
			// Act
			async Task Act() => await _dataProcessor.ProcessAndSaveAsync();

			// Assert
			await Assert.ThrowsAsync<InvalidDataException>(Act);
		}
	}
}
