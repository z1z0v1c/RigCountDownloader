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
			if (_configuration["FileName"].EndsWith(".xlsx"))
			{
				return new ExcelToCsvConverter(_configuration);
			}

			throw new Exception("Wrong input file type");
		}
	}
}
