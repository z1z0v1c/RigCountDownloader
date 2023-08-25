using Microsoft.Extensions.Configuration;
using RigCountDownloader.FileModificators;

namespace RigCountDownloader.StreamProcessors
{
    public class StreamProcessorFactory
    {
        private readonly IConfiguration _configuration;
        private readonly FileModificatorFactory _modificatorFactory;

        public StreamProcessorFactory(IConfiguration configuration, FileModificatorFactory modificatorFactory)
        {
            _configuration = configuration;
            _modificatorFactory = modificatorFactory;
        }

        public IStreamProcessor CreateStreamProcessor()
        {
            // Based on response MediaType instead of configuration
            if (_configuration["FileName"].EndsWith(".xlsx"))
            {
                return new ExcelStreamProcessor(_configuration, _modificatorFactory.CreateModificator());
            }

            throw new Exception("Wrong input file type");
        }
    }
}
