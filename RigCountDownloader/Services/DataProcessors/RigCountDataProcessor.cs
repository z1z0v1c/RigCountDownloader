using OfficeOpenXml;

namespace RigCountDownloader.Services.DataProcessors
{
    public class RigCountDataProcessor(ILogger logger, IFileWriter fileWriter, ExcelPackage excelPackage)
        : ExcelDataProcessor(logger, fileWriter, excelPackage)
    {
        public override async Task ProcessAndSaveDataAsync(CancellationToken cancellationToken = default)
        {
            var worksheet =
                ExcelPackage.Workbook.Worksheets.Count > 0 ? ExcelPackage.Workbook.Worksheets[0] : null;

            var startRowIndex = FindRowIndex(worksheet, "Europe");
            var endRowIndex = FindNthRowIndex(worksheet, "Avg.", 2, startRowIndex);

            for (var row = startRowIndex; row <= endRowIndex; row++)
            {
                var cellValues = new List<string>();
                for (var column = 1; column <= worksheet?.Dimension?.Columns; column++)
                {
                    var cellValue = worksheet.Cells[row, column].Text;
                    cellValues.Add(cellValue);
                }

                // Keep performance on mind
                await FileWriter.WriteLineAsync(string.Join(",", cellValues), cancellationToken);
            }

            Logger.Information("The CSV file saved to a file.");

            await FileWriter.DisposeAsync();
        }

        private static int FindRowIndex(ExcelWorksheet? worksheet, string searchValue, int startIndex = 1)
        {
            return FindNthRowIndex(worksheet, searchValue, 1, startIndex);
        }

        private static int FindNthRowIndex(ExcelWorksheet? worksheet, string searchValue, int n, int startIndex = 1)
        {
            var count = 0;
            for (var row = startIndex; row <= worksheet?.Dimension?.End.Row; row++)
            {
                for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    if (string.Equals(worksheet.Cells[row, col].Text, searchValue, StringComparison.OrdinalIgnoreCase))
                    {
                        count++;
                        if (count == n)
                        {
                            return row;
                        }
                    }
                }
            }

            throw new InvalidDataException($"{n}. occurrence of {searchValue} not found in the Excel file.");
        }
    }
}