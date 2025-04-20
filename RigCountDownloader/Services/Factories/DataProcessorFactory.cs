using OfficeOpenXml;
using RigCountDownloader.Domain.Interfaces;
using RigCountDownloader.Domain.Interfaces.Services;
using RigCountDownloader.Domain.Interfaces.Services.Factories;
using RigCountDownloader.Services.DataProcessors;
using Serilog;

namespace RigCountDownloader.Services.Factories
{
	public class DataProcessorFactory(ILogger logger) : IDataProcessorFactory
	{
		public IDataProcessor CreateDataProcessor(IFileWriter fileWriter, IConvertedData data)
		{
			if (data.FileType == "xlsx" && data.FileName!.Contains("Worldwide Rig Count"))
			{
				return new RigCountDataProcessor(logger, fileWriter, (ExcelPackage) data.Data);
			}

			throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
		}
	}
}
