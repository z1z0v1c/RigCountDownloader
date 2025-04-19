using RigCountDownloader.FileConverters;
using RigCountDownloader.StreamProcessors;

namespace RigCountDownloader.Services.Factories
{
	public class DataConverterFactory(IDataProcessorFactory dataProcessorFactory) : IDataConverterFactory
	{
		public IDataConverter CreateDataConverter(Data data)
		{
			if (data.MediaType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
			{
				return new XlsxDataConverter(dataProcessorFactory, data);
			}

			throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
		}
	}
}
