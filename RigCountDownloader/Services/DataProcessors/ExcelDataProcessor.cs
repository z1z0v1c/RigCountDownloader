using OfficeOpenXml;

namespace RigCountDownloader.FileConverters
{
	public abstract class ExcelDataProcessor : IDataProcessor
	{
		// Nullable?
		public ExcelPackage ExcelPackage { get; set; } = new ExcelPackage();

		public abstract Task ProcessAndSaveAsync();
	}
}
