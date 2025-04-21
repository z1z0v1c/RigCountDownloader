using OfficeOpenXml;
using RigCountDownloader.Domain.Interfaces;
using RigCountDownloader.Domain.Models;

namespace RigCountDownloader.Services.DataConverters
{
	public class XlsxDataFormater : IDataFormater
	{
		public XlsxDataFormater()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		}

        public FormatedData FormatData(DataStream dataStream)
        {
            return new FormatedData("xlsx", new ExcelPackage(dataStream.MemoryStream));
        }
    }
}
