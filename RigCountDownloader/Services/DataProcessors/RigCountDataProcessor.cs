using OfficeOpenXml;

namespace RigCountDownloader.Services.DataProcessors
{
    public class RigCountDataProcessor(IFileWriter fileWriter, ExcelPackage excelPackage)
        : ExcelDataProcessor(fileWriter, excelPackage)
    {
        public override async Task ProcessAndSaveDataAsync(Options options,
            CancellationToken cancellationToken = default)
        {
            ExcelWorksheet worksheet = ExcelPackage?.Workbook?.Worksheets?.Count > 0
                ? ExcelPackage.Workbook.Worksheets[0]
                : throw new InvalidDataException("ExcelWorksheet not found");

            if (worksheet.Dimension == null)
            {
                throw new InvalidDataException("ExcelWorksheet dimension not found");
            }

            string startCellValue = options.StartYear.ToString();
            string endCellValue = "Avg.";

            int startRowIndex = FindRowIndex(worksheet, startCellValue);
            int endRowIndex = FindNthRowIndex(worksheet, endCellValue, options.YearCount, startRowIndex);

            for (int row = startRowIndex; row <= endRowIndex; row++)
            {
                List<string> cellValues = [];
                for (int column = 1; column <= worksheet.Dimension.Columns; column++)
                {
                    var cellValue = worksheet.Cells[row, column].Text;
                    cellValues.Add(cellValue);
                }

                // Remove trailing commas only
                var line = string.Join(",", cellValues);
                line = line[..^1];
                
                // Keep performance on mind
                await FileWriter.WriteLineAsync(line, cancellationToken);
            }

            await FileWriter.DisposeAsync();
        }

        private static int FindRowIndex(ExcelWorksheet worksheet, string searchValue, int startIndex = 1)
        {
            return FindNthRowIndex(worksheet, searchValue, 1, startIndex);
        }

        private static int FindNthRowIndex(ExcelWorksheet worksheet, string searchValue, int n, int startIndex = 1)
        {
            int count = 0;
            ExcelCellAddress bottomRightCell = worksheet.Dimension.End;

            for (int row = startIndex; row <= bottomRightCell.Row; row++)
            {
                for (int col = 1; col <= bottomRightCell.Column; col++)
                {
                    if (searchValue != worksheet.Cells[row, col].Text)
                    {
                        continue;
                    }

                    count++;
                    if (count == n)
                    {
                        return row;
                    }
                }
            }

            throw new InvalidDataException($"{n}. occurrence of {searchValue} not found in the Excel file.");
        }
    }
}