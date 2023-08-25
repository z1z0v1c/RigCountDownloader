using OfficeOpenXml;

namespace RigCountDownloader.FileConverters
{
	public abstract class ExcelFileConverter : IFileConverter
	{
		// Nullable?
		public ExcelPackage ExcelPackage { get; set; } = new ExcelPackage();

		public abstract Task ConvertAndSaveAsync();
	}
}
