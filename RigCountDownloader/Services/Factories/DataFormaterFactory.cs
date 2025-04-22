using RigCountDownloader.Domain.Interfaces;
using RigCountDownloader.Domain.Interfaces.Factories;
using RigCountDownloader.Domain.Models.Enums;
using RigCountDownloader.Services.DataConverters;

namespace RigCountDownloader.Services.Factories
{
	public class DataFormaterFactory() : IDataFormaterFactory
	{
		public IDataFormater CreateDataFormater(string mediaType)
		{
			if (mediaType == MediaType.Spreadsheet)
			{
				return new XlsxDataFormater();
			}

			throw new ArgumentException("The file media type is missing or not supported.");
		}
	}
}
