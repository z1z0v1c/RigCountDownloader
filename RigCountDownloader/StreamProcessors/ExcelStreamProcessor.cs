using OfficeOpenXml;
using RigCountDownloader.FileConverters;
using RigCountDownloader.FileModificators;

namespace RigCountDownloader.StreamProcessors
{
	public class ExcelStreamProcessor : IStreamProcessor
	{
		private readonly IFileModificator _excelModificator;
		private readonly FileConverterFactory _fileConverterFactory;
		private readonly ExcelFileConverter _fileConverter;

		public ExcelStreamProcessor(IFileModificator excelModificator, FileConverterFactory fileConverterFactory)
		{
			_excelModificator = excelModificator;
			_fileConverterFactory = fileConverterFactory;
			_fileConverter = (ExcelFileConverter)_fileConverterFactory.CreateFileConverter();
		}

		public async Task ProcessStreamAsync(Stream stream)
		{
			using ExcelPackage package = new(stream);

			_excelModificator.ModifyFile(package);

			_fileConverter.ExcelPackage = package;

			await _fileConverter.ConvertAndSaveAsync();

			// Add logger
			Console.WriteLine($"File downloaded successfully.");

			// Implement IDisposable
			stream.Dispose();
		}
	}
}
