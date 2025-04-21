using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using OfficeOpenXml;
using RigCountDownloader.Domain.Interfaces.Services;
using RigCountDownloader.Services.DataProcessors;
using RigCountDownloader.Services.FileWriters;
using Serilog;
using Xunit;

namespace RigCountDownloader.Tests.Services.DataProcessors
{
    public class RigCountDataProcessorTests : TestFixture
    {
        private readonly ILogger _logger; 
        private IFileWriter? FileWriter { get; set; }
        private RigCountDataProcessor? DataProcessor { get; set; }

        public RigCountDataProcessorTests()
        {
            _logger = ServiceProvider.GetRequiredService<ILogger>();
        }

        [Fact]
        public async Task ProcessAndSaveAsync_ValidData_WritesToCsvFile()
        {
            // Arrange
            const string fileName = "Worldwide Rig Count Jul 2013.csv";
            FileWriter = new CsvFileWriter(fileName);
            DataProcessor = new RigCountDataProcessor(_logger, FileWriter, new ExcelPackage());
			
            var worksheet = DataProcessor.ExcelPackage.Workbook.Worksheets.Add("Sheet");
            worksheet.Cells[1, 1].Value = "Europe";
            worksheet.Cells[2, 1].Value = "Avg.";
            worksheet.Cells[3, 1].Value = "Avg.";

            // Act
            await DataProcessor.ProcessAndSaveAsync();

            // Assert
            _logger.Received().Information("The CSV file saved to a file.");
        }

        [Fact]
        public async Task ProcessAndSaveAsync_InvalidData_ThrowsInvalidDataException()
        {
            // Arrange
            const string fileName = "Worldwide Rig Count Jul 2014.csv";
            FileWriter = new CsvFileWriter(fileName);
            DataProcessor = new RigCountDataProcessor(_logger, FileWriter, new ExcelPackage());

            var worksheet = DataProcessor.ExcelPackage.Workbook.Worksheets.Add("Sheet");
            worksheet.Cells[1, 1].Value = "Europe";
            worksheet.Cells[2, 1].Value = "Europe";
            worksheet.Cells[3, 1].Value = "Avg.";

            // Act
            async Task Act() => await DataProcessor.ProcessAndSaveAsync();

            // Assert
            await Assert.ThrowsAsync<InvalidDataException>(Act);
        }

        [Fact]
        public async Task ProcessAndSaveAsync_EmptyWorksheet_ThrowsInvalidDataException()
        {
            // Arrange
            const string fileName = "Worldwide Rig Count Jul 2015.csv";
            FileWriter = new CsvFileWriter(fileName);
            DataProcessor = new RigCountDataProcessor(_logger, FileWriter, new ExcelPackage());

            DataProcessor.ExcelPackage.Workbook.Worksheets.Add("EmptyWorksheet");
			
            // Act
            async Task Act() => await DataProcessor.ProcessAndSaveAsync();

            // Assert
            await Assert.ThrowsAsync<InvalidDataException>(Act);
        }

        [Fact]
        public async Task  ProcessAndSaveAsync_NoData_ThrowsInvalidDataException()
        {
            // Arrange
            const string fileName = "Worldwide Rig Count Jul 2016.csv";
            FileWriter = new CsvFileWriter(fileName);
            DataProcessor = new RigCountDataProcessor(_logger, FileWriter, new ExcelPackage());

            using var memoryStream = new MemoryStream();
            using var package = new ExcelPackage();

            DataProcessor.ExcelPackage = package;
			
            // Act
            async Task Act() => await DataProcessor.ProcessAndSaveAsync();

            // Assert
            await Assert.ThrowsAsync<InvalidDataException>(Act);
        }
    }
}