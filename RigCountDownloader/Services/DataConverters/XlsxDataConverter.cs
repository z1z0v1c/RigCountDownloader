using OfficeOpenXml;
using RigCountDownloader.FileConverters;

namespace RigCountDownloader.StreamProcessors
{
	public class XlsxDataConverter : IDataConverter
	{
		private readonly IDataProcessorFactory _dataProcessorFactory;
		private readonly ExcelDataProcessor _dataProcessor;

		public XlsxDataConverter(IDataProcessorFactory dataProcessorFactory, Response response)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			_dataProcessorFactory = dataProcessorFactory;
			_dataProcessor = (ExcelDataProcessor)_dataProcessorFactory.CreateFileConverter(response);
		}

		public async Task ConvertDataAsync(Stream stream)
		{
			using ExcelPackage package = new(stream);

			_dataProcessor.ExcelPackage = package;

			await _dataProcessor.ProcessAndSaveAsync();
		}
	}
}
