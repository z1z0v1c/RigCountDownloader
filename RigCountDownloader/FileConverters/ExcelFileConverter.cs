using OfficeOpenXml;

namespace RigCountDownloader.FileConverters
{
	public abstract class ExcelFileConverter : IFileConverter
	{
		public ExcelPackage ExcelPackage { get; set; } = new ExcelPackage();

		public abstract Task ConvertAndSaveAsync();
	}
}
