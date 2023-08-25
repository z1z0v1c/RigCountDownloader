using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace RigCountDownloader.FileConverters
{
	public class WWRCExcelToCsvConverter : ExcelFileConverter
	{
		private readonly IConfiguration _configuration;

		public WWRCExcelToCsvConverter(IConfiguration configuration)
		{
			this._configuration = configuration;
		}

		public override async Task ConvertAndSaveAsync()
		{
			await ConvertAndSaveAsync(ExcelPackage);
		}

		public async Task ConvertAndSaveAsync(ExcelPackage package)
		{
			string outputFilePath = $"{Directory.GetCurrentDirectory()}/{_configuration["OutputFileName"]}";
			using StreamWriter writer = new(outputFilePath);

			// Apply for each worksheet?
			ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

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
