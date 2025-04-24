using RigCountProcessor.Services.DataFormatters;

namespace RigCountProcessor.Services.Factories;

public class DataFormatterFactory() : IDataFormaterFactory
{
    public IDataFormatter CreateDataFormatter(string mediaType)
    {
        if (mediaType == MediaType.Spreadsheet)
        {
            return new XlsxDataFormatter();
        }

        throw new ArgumentException("The file media type is missing or not supported.");
    }
}