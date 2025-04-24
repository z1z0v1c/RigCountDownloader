using OfficeOpenXml;
using RigCountProcessor.Services.DataProcessors;

namespace RigCountProcessor.Services.Factories;

public class DataProcessorFactory() : IDataProcessorFactory
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
            return new RigCountDataProcessor(fileWriter, (ExcelPackage)data);
        }

        throw new IncorrectSettingsException(
            "Wrong or missing Context and/or SourceFileFormat settings. Check appsettings.json file.");
    }
}