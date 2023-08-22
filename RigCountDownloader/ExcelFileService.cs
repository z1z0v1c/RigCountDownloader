using OfficeOpenXml;

namespace RigCountDownloader
{
	public class ExcelFileService : IFileService
	{
		public async Task WriteToFileAsync(HttpContent content)
		{
			using Stream stream = await content.ReadAsStreamAsync();
			using ExcelPackage package = new(stream);

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			var worksheet = package.Workbook.Worksheets[0];

			// Find the row index of the first occurrence of 'Europe'
			int startRowIndex = FindRowIndex(worksheet, "Europe");
			if (startRowIndex == -1)
			{
				Console.WriteLine("Europe not found in the Excel file.");
				return;
			}

			// Find the row index of the second occurrence of 'Avg.'
			int endRowIndex = FindNthRowIndex(worksheet, "Avg.", 2,  startRowIndex);
			if (endRowIndex == -1)
			{
				Console.WriteLine("Second occurrence of Avg. not found in the Excel file.");
				return;
			}

			string currentDirectory = Directory.GetCurrentDirectory();

			// Specify the relative path
			string relativePath = "..\\..\\..\\..\\Worldwide Rig Count Jul 2023.csv";

			// Combine the current directory and relative path for the CSV file
			string csvFilePath = Path.Combine(currentDirectory, relativePath);

			using (var writer = new StreamWriter(csvFilePath))
			{
				// Iterate through the rows and write to the CSV file
				for (int row = startRowIndex; row <= endRowIndex; row++)
				{
					string csvLine = string.Empty;
					for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
					{
						csvLine += worksheet.Cells[row, col].Value + ",";
					}
					await writer.WriteLineAsync(csvLine[..^1]);
				}
			}
		}

		static int FindRowIndex(ExcelWorksheet worksheet, string searchValue, int startIndex = 1)
		{
			return FindNthRowIndex(worksheet, searchValue, 1, startIndex);
		}

		static int FindNthRowIndex(ExcelWorksheet worksheet, string searchValue, int n, int startIndex = 1)
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
			return -1;
		}
	}
}
