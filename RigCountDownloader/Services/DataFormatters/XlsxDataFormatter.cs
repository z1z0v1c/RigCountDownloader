using OfficeOpenXml;

namespace RigCountDownloader.Services.DataFormatters
{
    public class XlsxDataFormatter : IDataFormatter
    {
        public XlsxDataFormatter()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public FormattedData FormatData(DataStream dataStream)
        {
            return new FormattedData("xlsx", new ExcelPackage(dataStream.MemoryStream));
        }
    }
}