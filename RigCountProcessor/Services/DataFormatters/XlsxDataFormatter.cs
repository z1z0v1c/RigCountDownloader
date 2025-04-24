using OfficeOpenXml;

namespace RigCountProcessor.Services.DataFormatters;

public class XlsxDataFormatter : IDataFormatter
{
    public XlsxDataFormatter()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public FormattedData FormatData(DataStream dataStream)
    {
        return new FormattedData(FileFormat.Xlsx, new ExcelPackage(dataStream.MemoryStream));
    }
}