using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace RigCountDownloader.FileModificators
{
    internal class ExcelModificator : IFileModificator
    {
        private readonly IConfiguration _configuration;

        public ExcelModificator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ModifyFile(ExcelPackage package)
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            int startRowIndex = FindRowIndex(worksheet, "Europe");
            int endRowIndex = FindNthRowIndex(worksheet, "Avg.", 2, startRowIndex);

            for (int rowIndex = worksheet.Dimension.End.Row; rowIndex >= 1; rowIndex--)
            {
                if (rowIndex < startRowIndex || rowIndex > endRowIndex)
                {
                    worksheet.DeleteRow(rowIndex);
                }
            }
        }

        private static int FindRowIndex(ExcelWorksheet worksheet, string searchValue, int startIndex = 1)
        {
            return FindNthRowIndex(worksheet, searchValue, 1, startIndex);
        }

        private static int FindNthRowIndex(ExcelWorksheet worksheet, string searchValue, int n, int startIndex = 1)
        {
            int count = 0;
            for (int row = startIndex; row <= worksheet.Dimension.End.Row; row++)
            {
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
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

            throw new ArgumentException($"{n}. occurrence of {searchValue} not found in the Excel file.");
        }
    }
}