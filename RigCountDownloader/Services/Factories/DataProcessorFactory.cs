using Microsoft.Extensions.Configuration;
using Serilog;

namespace RigCountDownloader.FileConverters
{
	public class DataProcessorFactory(ILogger logger, IConfiguration configuration) : IDataProcessorFactory
	{
		public IDataProcessor CreateFileConverter(Data data)
		{
			// Based on the response MediaType instead of configuration for the InputFileName
			if (data.MediaType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" &&
				data.SourceFileName!.Contains("Worldwide Rig Count") &&
				configuration["OutputFileType"] == "csv")
			{
				return new RigCountDataProcessor(logger, configuration);
			}

			throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
		}
	}
}
