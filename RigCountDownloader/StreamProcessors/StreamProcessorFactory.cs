using Microsoft.Extensions.Configuration;
using RigCountDownloader.FileConverters;
using RigCountDownloader.FileModificators;

namespace RigCountDownloader.StreamProcessors
{
	public class StreamProcessorFactory
	{
		private readonly IConfiguration _configuration;
		private readonly FileModificatorFactory _modificatorFactory;
		private readonly FileConverterFactory _fileConverterFactory;

		public StreamProcessorFactory(IConfiguration configuration, FileModificatorFactory modificatorFactory, FileConverterFactory fileConverterFactory)
		{
			_configuration = configuration;
			_modificatorFactory = modificatorFactory;
			_fileConverterFactory = fileConverterFactory;
		}

		public IStreamProcessor CreateStreamProcessor()
		{
			// Based on the response MediaType instead of configuration
			if (_configuration["InputFileName"].EndsWith(".xlsx"))
			{
				return new ExcelStreamProcessor(_modificatorFactory.CreateFileModificator(), _fileConverterFactory);
			}

			throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
		}
	}
}
