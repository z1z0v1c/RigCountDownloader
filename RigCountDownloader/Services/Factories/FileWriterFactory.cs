using RigCountDownloader.Domain.Interfaces;
using RigCountDownloader.Domain.Interfaces.Factories;
using RigCountDownloader.Domain.Models.Enums;
using RigCountDownloader.Services.FileWriters;

namespace RigCountDownloader.Services.Factories;

public class FileWriterFactory : IFileWriterFactory
{
    public IFileWriter CreateFileWriter(string fileFormat, string fileLocation)
    {
        if (fileFormat == FileFormat.Csv)
        {
            return new CsvFileWriter(fileLocation);
        }
        
        throw new ArgumentException("Wrong output file type or location. Check appsettings.json file.");
    }
}