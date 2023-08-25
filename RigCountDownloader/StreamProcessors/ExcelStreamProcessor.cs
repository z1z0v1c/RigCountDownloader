using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using RigCountDownloader.FileConverters;
using RigCountDownloader.FileModificators;

namespace RigCountDownloader.StreamProcessors
{
    public class ExcelStreamProcessor : IStreamProcessor
    {
        private readonly IFileModificator _excelModificator;
        private readonly FileConverterFactory _fileConverterFactory;

		public ExcelStreamProcessor(IFileModificator excelModificator, FileConverterFactory fileConverterFactory)
		{
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
