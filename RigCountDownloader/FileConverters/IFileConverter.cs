using OfficeOpenXml;

namespace RigCountDownloader.FileConverters
{
	public interface IFileConverter
	{
		Task ConvertAndSaveAsync(ExcelPackage package);
	}
}
