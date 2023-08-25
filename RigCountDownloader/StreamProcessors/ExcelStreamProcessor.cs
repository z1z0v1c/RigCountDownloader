using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using RigCountDownloader.FileModificators;

namespace RigCountDownloader.StreamProcessors
{
    public class ExcelStreamProcessor : IStreamProcessor
    {
        private readonly IConfiguration _configuration;
        private readonly IExcelModificator _excelModificator;

        public ExcelStreamProcessor(IConfiguration configuration, IExcelModificator excelModificator)
        {
            _configuration = configuration;
            _excelModificator = excelModificator;
        }

        public async Task ProcessStreamAsync(Stream stream)
        {
            using ExcelPackage package = new(stream);

            _excelModificator.Modify(package);

            await SaveToCsvFileAsync(package);

            Console.WriteLine($"File downloaded successfully.");

            // Implement IDisposable
            stream.Dispose();
        }

        private async Task SaveToCsvFileAsync(ExcelPackage package)
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            string currentDirectory = Directory.GetCurrentDirectory();

            string csvFilePath = Path.Combine(currentDirectory, _configuration["RelativeOutputPath"]);

            using StreamWriter writer = new(csvFilePath);
            for (int row = 1; row <= worksheet.Dimension.Rows; row++)
            {
                var cellValues = new List<string>();
                for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                {
                    var cellValue = worksheet.Cells[row, col].Text;
                    cellValues.Add(cellValue);
                }
                await writer.WriteLineAsync(string.Join(",", cellValues));
            }
        }
    }
}
