using RigCountDownloader.Services.FileWriters;

namespace RigCountDownloader.Services.Factories;

public class FileWriterFactory : IFileWriterFactory
{
    public IFileWriter CreateFileWriter(string fileFormat, string fileLocation)
    {
        if (string.IsNullOrEmpty(fileLocation))
        {
            throw new IncorrectSettingsException("Missing OutputFileLocation setting. Check appsettings.json file.");
        }

        if (fileFormat == FileFormat.Csv)
        {
            return new CsvFileWriter(fileLocation);
        }

        throw new IncorrectSettingsException("Wrong or missing OutputFileFormat setting. Check appsettings.json file.");
    }
}