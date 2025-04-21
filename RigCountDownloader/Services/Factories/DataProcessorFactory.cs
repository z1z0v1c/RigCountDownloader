using OfficeOpenXml;
using RigCountDownloader.Domain.Interfaces.Services;
using RigCountDownloader.Domain.Interfaces.Services.Factories;
using RigCountDownloader.Domain.Models.Enums;
using RigCountDownloader.Services.DataProcessors;
using Serilog;

namespace RigCountDownloader.Services.Factories
{
    public class DataProcessorFactory(ILogger logger) : IDataProcessorFactory
    {
        public IDataProcessor CreateDataProcessor(
            IFileWriter fileWriter,
            string fileFormat,
            string fileName,
            object data
        )
        {
            if (fileFormat == FileFormat.Xlsx && fileName.Contains("Worldwide Rig Count"))
            {
                return new RigCountDataProcessor(logger, fileWriter, (ExcelPackage)data);
            }

            throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
        }
    }
}