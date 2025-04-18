using Microsoft.Extensions.Configuration;
using RigCountDownloader.FileConverters;

namespace RigCountDownloader.StreamProcessors
{
	public class StreamProcessorFactory(IConfiguration configuration, IFileConverterFactory fileConverterFactory)
    {
		private readonly IConfiguration _configuration = configuration;
		private readonly IFileConverterFactory _fileConverterFactory = fileConverterFactory;

        public IStreamProcessor CreateStreamProcessor(Response response)
		{
			// Based on the response MediaType instead of configuration
			if (response.MediaType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
			{
				return new ExcelStreamProcessor(_fileConverterFactory, response);
			}

			throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
		}
	}
}
