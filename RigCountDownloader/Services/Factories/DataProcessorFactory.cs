using Microsoft.Extensions.Configuration;
using Serilog;

namespace RigCountDownloader.FileConverters
{
	public class DataProcessorFactory(ILogger logger, IConfiguration configuration) : IDataProcessorFactory
	{
		public IDataProcessor CreateFileConverter(Response response)
		{
			// Based on the response MediaType instead of configuration for the InputFileName
			if (response.MediaType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" &&
				response.FileName!.Contains("Worldwide Rig Count") &&
				configuration["OutputFileType"] == "csv")
			{
				return new RigCountDataProcessor(logger, configuration);
			}

			throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
		}
	}
}
