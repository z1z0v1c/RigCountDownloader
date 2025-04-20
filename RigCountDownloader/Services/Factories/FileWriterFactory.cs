using RigCountDownloader.Domain.Interfaces;
using RigCountDownloader.Domain.Interfaces.Services;
using RigCountDownloader.Domain.Interfaces.Services.Factories;
using RigCountDownloader.Services.FileWriters;

namespace RigCountDownloader.Services.Factories;

public class FileWriterFactory : IFileWriterFactory
{
    public IFileWriter CreateFileWriter(string fileType, string fileLocation)
    {
        if (fileType.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
        {
            return new CsvFileWriter(fileLocation);
        }
        
        throw new ArgumentException("Wrong output file type or location. Check appsettings.json file.");
    }
}