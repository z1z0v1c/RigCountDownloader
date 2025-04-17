using Microsoft.Extensions.Configuration;
using RigCountDownloader.FileConverters;

namespace RigCountDownloader.StreamProcessors
{
	public class StreamProcessorFactory(IConfiguration configuration, IFileConverterFactory fileConverterFactory)
    {
		private readonly IConfiguration _configuration = configuration;
		private readonly IFileConverterFactory _fileConverterFactory = fileConverterFactory;

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
