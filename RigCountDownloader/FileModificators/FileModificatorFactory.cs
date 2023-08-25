using Microsoft.Extensions.Configuration;

namespace RigCountDownloader.FileModificators
{
	public class FileModificatorFactory
	{
		private readonly IConfiguration _configuration;

		public FileModificatorFactory(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public IFileModificator CreateFileModificator()
		{
			// Based on the response MediaType instead of configuration
			if (_configuration["InputFileName"].EndsWith(".xlsx"))
			{
				return new ExcelModificator();
			}

			throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
		}
	}
}
