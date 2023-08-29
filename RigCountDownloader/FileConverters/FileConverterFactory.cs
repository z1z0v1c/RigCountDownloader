using Microsoft.Extensions.Configuration;
using Serilog;

namespace RigCountDownloader.FileConverters
{
	public class FileConverterFactory : IFileConverterFactory
	{
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;

		public FileConverterFactory(ILogger logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;
		}

		public IFileConverter CreateFileConverter()
		{
			// Based on the response MediaType instead of configuration for the InputFileName
			if (_configuration["InputFileName"].EndsWith(".xlsx") &&
				_configuration["OutputFileName"].StartsWith("Worldwide Rig Count") &&
				_configuration["OutputFileName"].EndsWith(".csv"))
			{
				return new WWRCExcelToCsvConverter(_logger, _configuration);
			}

			throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
		}
	}
}
