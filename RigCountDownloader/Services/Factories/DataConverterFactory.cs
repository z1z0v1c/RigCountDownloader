using RigCountDownloader.Domain.Interfaces.DataConverters;
using RigCountDownloader.Domain.Interfaces.Factories;
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
