﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using OfficeOpenXml;
using RigCountDownloader.FileConverters;
using Serilog;
using Xunit;

namespace RigCountDownloader.Tests.FileConverters
{
	public class RigCountDataProcessorTests : TestFixture
	{
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;
		private readonly RigCountDataProcessor _fileConverter;

		public RigCountDataProcessorTests()
		{
			_logger = ServiceProvider.GetRequiredService<ILogger>();
			_configuration = ServiceProvider.GetRequiredService<IConfiguration>();
			_fileConverter = ServiceProvider.GetRequiredService<RigCountDataProcessor>();
		}

		[Fact]
		public async Task ConvertAndSaveAsync_ValidData_WritesToCsvFile()
		{
			// Arrange
			string fileName = "WWRC Jul 2023.csv";
			string outputFilePath = $"{Directory.GetCurrentDirectory()}\\{fileName}";
			_configuration["OutputFileLocation"].Returns(fileName);

			using MemoryStream memoryStream = new();
			using ExcelPackage package = new(memoryStream);

			ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet");
			worksheet.Cells[1, 1].Value = "Europe";
			worksheet.Cells[2, 1].Value = "Avg.";
			worksheet.Cells[3, 1].Value = "Avg.";

			// Act
			await _fileConverter.ConvertAndSaveAsync(package);

			// Assert
			_logger.Received().Information($"The CSV file saved to {outputFilePath}.");
		}

		[Fact]
		public async Task ConvertAndSaveAsync_InalidData_ThrowsInvalidDataException()
		{
			// Arrange
			string fileName = "WWRC Jul 2023.csv";
			_configuration["OutputFileLocation"].Returns(fileName);

			using MemoryStream memoryStream = new();
			using ExcelPackage package = new(memoryStream);

			ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet");
			worksheet.Cells[1, 1].Value = "Europe";
			worksheet.Cells[2, 1].Value = "Europe";
			worksheet.Cells[3, 1].Value = "Avg.";

			// Act
			async Task act() => await _fileConverter.ConvertAndSaveAsync(package);

			// Assert
			await Assert.ThrowsAsync<InvalidDataException>(act);
		}

		[Fact]
		public async Task ConvertAndSaveAsync_EmptyWorksheet_ThrowsInvalidDataException()
		{
			// Arrange
			string fileName = "WWRC Jul 2023.csv";
			_configuration["OutputFileLocation"].Returns(fileName);

			using MemoryStream memoryStream = new();
			using ExcelPackage package = new(memoryStream);
			package.Workbook.Worksheets.Add("EmptyWorksheet");

			// Act
			async Task act() => await _fileConverter.ConvertAndSaveAsync(package);

			// Assert
			await Assert.ThrowsAsync<InvalidDataException>(act);
		}

		[Fact]
		public async Task ConvertAndSaveAsync_NoData_ThrowsInvalidDataException()
		{
			// Arrange
			string fileName = "WWRC Jul 2023.csv";
			_configuration["OutputFileLocation"].Returns(fileName);

			using MemoryStream memoryStream = new();
			using ExcelPackage package = new(memoryStream);

			// Act
			async Task act() => await _fileConverter.ConvertAndSaveAsync(package);

			// Assert
			await Assert.ThrowsAsync<InvalidDataException>(act);
		}
	}
}
