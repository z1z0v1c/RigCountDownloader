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
			if (_configuration["FileName"].EndsWith(".xlsx"))
            {
                return new ExcelModificator(_configuration);
            }

            throw new Exception("Wrong input file type");
        }
    }
}
