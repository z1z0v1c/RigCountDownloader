using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using RigCountDownloader.FileConverters;
using RigCountDownloader.FileModificators;

namespace RigCountDownloader.StreamProcessors
{
    public class ExcelStreamProcessor : IStreamProcessor
    {
        private readonly IConfiguration _configuration;
        private readonly IFileModificator _excelModificator;
        private readonly FileConverterFactory _fileConverterFactory;

		public ExcelStreamProcessor(IConfiguration configuration, IFileModificator excelModificator, FileConverterFactory fileConverterFactory)
		{
			_configuration = configuration;
			_excelModificator = excelModificator;
			this._fileConverterFactory = fileConverterFactory;
		}

		public async Task ProcessStreamAsync(Stream stream)
        {
            using ExcelPackage package = new(stream);

            _excelModificator.ModifyFile(package);

            IFileConverter fileConverter = _fileConverterFactory.CreateFileConverter();

            await fileConverter.ConvertAndSaveAsync(package);

            Console.WriteLine($"File downloaded successfully.");

            // Implement IDisposable
            stream.Dispose();
        }
    }
}
