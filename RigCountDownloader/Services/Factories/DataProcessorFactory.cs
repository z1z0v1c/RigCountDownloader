using OfficeOpenXml;
using RigCountDownloader.Domain.Interfaces;
using RigCountDownloader.Domain.Interfaces.Factories;
using RigCountDownloader.Domain.Models.Enums;
using RigCountDownloader.Services.DataProcessors;
using Serilog;

namespace RigCountDownloader.Services.Factories
{
    public class DataProcessorFactory(ILogger logger) : IDataProcessorFactory
    {
        public IDataProcessor CreateDataProcessor(
            IFileWriter fileWriter,
            string context,
            string fileFormat,
            object data
        )
        {
            if (context == Context.RigCount && fileFormat == FileFormat.Xlsx)
            {
                return new RigCountDataProcessor(logger, fileWriter, (ExcelPackage)data);
            }

            throw new ArgumentException("Wrong input file type. Check appsettings.json file.");
        }
    }
}