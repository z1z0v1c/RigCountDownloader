using OfficeOpenXml;
using RigCountDownloader.Services.DataProcessors;

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

            throw new IncorrectSettingsException(
                "Wrong or missing Context and/or SourceFileFormat settings. Check appsettings.json file.");
        }
    }
}