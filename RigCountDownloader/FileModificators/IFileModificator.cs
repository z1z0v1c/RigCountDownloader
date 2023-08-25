using OfficeOpenXml;

namespace RigCountDownloader.FileModificators
{
	public interface IFileModificator
	{
		void ModifyFile(ExcelPackage package);
	}
}