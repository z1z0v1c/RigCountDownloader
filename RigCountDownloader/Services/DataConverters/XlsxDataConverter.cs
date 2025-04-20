using OfficeOpenXml;
using RigCountDownloader.Domain.Interfaces;
using RigCountDownloader.Domain.Interfaces.Services;
using RigCountDownloader.Domain.Models;

namespace RigCountDownloader.Services.DataConverters
{
	public class XlsxDataConverter : IDataConverter
	{
		public XlsxDataConverter()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		}

        public IConvertedData ConvertData(Data data)
        {
            return new XlsxData("xlsx", data.FileName!, new ExcelPackage(data.MemoryStream));
        }
    }
}
