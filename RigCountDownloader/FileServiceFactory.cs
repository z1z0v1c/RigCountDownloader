using Microsoft.Extensions.Configuration;

namespace RigCountDownloader
{
	public class FileServiceFactory
	{
		private readonly IConfiguration _configuration;

		public FileServiceFactory(IConfiguration configuration)
		{
			this._configuration = configuration;
		}

		public IFileService CreateFileService()
		{
			if (_configuration["FileName"].EndsWith(".xlsx"))
			{
				return new ExcelFileService(_configuration);
			}

			throw new Exception("Wrong input file type");
		}
	}
}
