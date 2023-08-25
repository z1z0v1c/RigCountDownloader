using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace RigCountDownloader.FileConverters
{
	internal class ExcelToCsvConverter : IFileConverter
	{
		private readonly IConfiguration _configuration;

		public ExcelToCsvConverter(IConfiguration configuration)
		{
			this._configuration = configuration;
		}

		public async Task ConvertAndSaveAsync(ExcelPackage package)
		{
			ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

			string outputFilePath = $"{Directory.GetCurrentDirectory()}/{_configuration["OutputFileName"]}";

			using StreamWriter writer = new(outputFilePath);
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
