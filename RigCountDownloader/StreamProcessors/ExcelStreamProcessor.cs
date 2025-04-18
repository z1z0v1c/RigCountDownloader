using OfficeOpenXml;
using RigCountDownloader.FileConverters;

namespace RigCountDownloader.StreamProcessors
{
	public class ExcelStreamProcessor : IStreamProcessor
	{
		private readonly IFileConverterFactory _fileConverterFactory;
		private readonly ExcelFileConverter _fileConverter;

		public ExcelStreamProcessor(IFileConverterFactory fileConverterFactory, Response response)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			_fileConverterFactory = fileConverterFactory;
			_fileConverter = (ExcelFileConverter)_fileConverterFactory.CreateFileConverter(response);
		}

		public async Task ProcessStreamAsync(Stream stream)
		{
			using ExcelPackage package = new(stream);

			_fileConverter.ExcelPackage = package;

			await _fileConverter.ConvertAndSaveAsync();
		}
	}
}
