using Microsoft.Extensions.Configuration;

namespace RigCountDownloader.FileConverters
{
	public class FileConverterFactory
	{
		private readonly IConfiguration _configuration;

		public FileConverterFactory(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public IFileConverter CreateFileConverter()
		{
			// Based on the response MediaType instead of configuration for the InputFileName
			if (_configuration["InputFileName"].EndsWith(".xlsx") &&
				_configuration["OutputFileName"].StartsWith("Worldwide Rig Count") &&
				_configuration["OutputFileName"].EndsWith(".csv"))
			{
				return new WWRCExcelToCsvConverter(_configuration);
			}

			throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
		}
	}
}
