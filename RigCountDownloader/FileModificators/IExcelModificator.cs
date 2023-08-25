using OfficeOpenXml;

namespace RigCountDownloader.FileModificators
{
    public interface IExcelModificator
    {
        void Modify(ExcelPackage package);
    }
}