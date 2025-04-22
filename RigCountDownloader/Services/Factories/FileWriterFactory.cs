using RigCountDownloader.Domain.Interfaces;
using RigCountDownloader.Domain.Interfaces.Factories;
using RigCountDownloader.Domain.Models.Enums;
using RigCountDownloader.Domain.Models.Exceptions;
using RigCountDownloader.Services.FileWriters;

namespace RigCountDownloader.Services.Factories;

public class FileWriterFactory : IFileWriterFactory
{
    public IFileWriter CreateFileWriter(string fileFormat, string fileLocation)
    {
        if (fileLocation == null)
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