using Microsoft.Extensions.Configuration;
using Serilog;

namespace RigCountDownloader.FileConverters
{
	public class FileConverterFactory(ILogger logger, IConfiguration configuration) : IFileConverterFactory
	{
		private readonly ILogger _logger = logger;
		private readonly IConfiguration _configuration = configuration;

        public IFileConverter CreateFileConverter(Response response)
		{
			// Based on the response MediaType instead of configuration for the InputFileName
			if (response.MediaType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" &&
				response.FileName!.Contains("Worldwide Rig Count") &&
				_configuration["OutputFileName"]!.EndsWith(".csv"))
			{
				return new WWRCExcelToCsvConverter(_logger, _configuration);
			}

			throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
		}
	}
}
