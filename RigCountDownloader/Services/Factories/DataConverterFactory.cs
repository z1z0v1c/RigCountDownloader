using RigCountDownloader.Domain.Interfaces;
using RigCountDownloader.Domain.Interfaces.Services;
using RigCountDownloader.Domain.Interfaces.Services.Factories;
using RigCountDownloader.Services.DataConverters;

namespace RigCountDownloader.Services.Factories
{
	public class DataConverterFactory() : IDataConverterFactory
	{
		public IDataConverter CreateDataConverter(string mediaType)
		{
			if (mediaType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
			{
				return new XlsxDataConverter();
			}

			throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
		}
	}
}
