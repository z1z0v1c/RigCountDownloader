﻿using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using Serilog;

namespace RigCountDownloader.FileConverters
{
	public class WWRCExcelToCsvConverter : ExcelFileConverter
	{
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;

		public WWRCExcelToCsvConverter(ILogger logger, IConfiguration configuration)
		{
			this._logger = logger;
			this._configuration = configuration;
		}

		public override async Task ConvertAndSaveAsync()
		{
			await ConvertAndSaveAsync(ExcelPackage);
		}

		public async Task ConvertAndSaveAsync(ExcelPackage package)
		{
			_logger.Information("Converting the Excel file to a CSV file...");

			string outputFilePath = $"{Directory.GetCurrentDirectory()}\\{_configuration["OutputFileName"]}";
			using StreamWriter writer = new(outputFilePath);

			// Apply for each worksheet?
			ExcelWorksheet? worksheet = package.Workbook.Worksheets.Count > 0 ?
											package.Workbook.Worksheets[0] : null;

			// Could be improved?
			int startRowIndex = FindRowIndex(worksheet, "Europe");
			int endRowIndex = FindNthRowIndex(worksheet, "Avg.", 2, startRowIndex);

			for (int row = startRowIndex; row <= endRowIndex; row++)
			{
				var cellValues = new List<string>();
				for (int col = 1; col <= worksheet?.Dimension?.Columns; col++)
				{
					var cellValue = worksheet.Cells[row, col].Text;
					cellValues.Add(cellValue);
				}
				await writer.WriteLineAsync(string.Join(",", cellValues));
			}

			_logger.Information($"The CSV file saved to {outputFilePath}.");
		}

		private static int FindRowIndex(ExcelWorksheet? worksheet, string searchValue, int startIndex = 1)
		{
			return FindNthRowIndex(worksheet, searchValue, 1, startIndex);
		}

		private static int FindNthRowIndex(ExcelWorksheet? worksheet, string searchValue, int n, int startIndex = 1)
		{
			int count = 0;
			for (int row = startIndex; row <= worksheet?.Dimension?.End.Row; row++)
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

			throw new InvalidDataException($"{n}. occurrence of {searchValue} not found in the Excel file.");
		}
	}
}
