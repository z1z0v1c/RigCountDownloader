using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using RigCountDownloader.Domain.Interfaces;
using RigCountDownloader.Domain.Interfaces.DataProcessors;
using RigCountDownloader.Domain.Interfaces.Factories;
using RigCountDownloader.Services.DataProcessors;
using Serilog;

namespace RigCountDownloader.Services.Factories
{
	public class DataProcessorFactory(ILogger logger, IConfiguration configuration) : IDataProcessorFactory
	{
		public IDataProcessor CreateDataProcessor(IConvertedData data)
		{
			// Based on the response MediaType instead of configuration for the InputFileName
			if (data.FileType == "xlsx" &&
				data.FileName!.Contains("Worldwide Rig Count")) // &&
				// configuration["OutputFileType"] == "csv")
			{
				return new RigCountDataProcessor(logger, configuration, (ExcelPackage) data.Data);
			}

			throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
		}
	}
}
