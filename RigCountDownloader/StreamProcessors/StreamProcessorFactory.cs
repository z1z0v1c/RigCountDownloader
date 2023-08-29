using Microsoft.Extensions.Configuration;
using RigCountDownloader.FileConverters;

namespace RigCountDownloader.StreamProcessors
{
	public class StreamProcessorFactory
	{
		private readonly IConfiguration _configuration;
		private readonly IFileConverterFactory _fileConverterFactory;

		public StreamProcessorFactory(IConfiguration configuration, IFileConverterFactory fileConverterFactory)
		{
			_configuration = configuration;
			_fileConverterFactory = fileConverterFactory;
		}

		public IStreamProcessor CreateStreamProcessor()
		{
			// Based on the response MediaType instead of configuration
			if (_configuration["InputFileName"].EndsWith(".xlsx"))
			{
				return new ExcelStreamProcessor(_fileConverterFactory);
			}

			throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
		}
	}
}
