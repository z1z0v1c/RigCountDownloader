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
			// Based on response MediaType instead of configuration
			if (_configuration["InputFileName"].EndsWith(".xlsx"))
			{
				return new ExcelToCsvConverter(_configuration);
			}

			throw new ArgumentException("Wrong input file type");
		}
	}
}
