namespace RigCountDownloader.Tests.Services.Factories;

public class FileWriterFactoryTests : TestFixture
{
    private readonly IFileWriterFactory _fileWriterFactory;

    public FileWriterFactoryTests()
    {
        _fileWriterFactory = ServiceProvider.GetRequiredService<IFileWriterFactory>();
    }

    [Fact]
    public void CreateFileWriter_ValidSettings_ReturnsFileWriter()
    {
        // Arrange
        const string fileFormat = FileFormat.Csv;
        const string fileLocation = "./valid_location.csv";

        // Act
        var fileWriter = _fileWriterFactory.CreateFileWriter(fileFormat, fileLocation);

        // Assert
        Assert.IsType<CsvFileWriter>(fileWriter);
    }

    [Fact]
    public void CreateFileWriter_InvalidFileFormat_ThrowsInvalidSettingsException()
    {
        // Arrange
        const string fileFormat = "odt";
        const string fileLocation = "./valid_location.csv";

        // Act
        void Act() => _fileWriterFactory.CreateFileWriter(fileFormat, fileLocation);

        // Assert
        Assert.Throws<IncorrectSettingsException>(Act);
    }

    [Fact]
    public void CreateFileWriter_NullFileLocation_ThrowsInvalidSettingsException()
    {
        // Arrange
        const string fileFormat = FileFormat.Csv;
        const string? fileLocation = null;

        // Act
        void Act() => _fileWriterFactory.CreateFileWriter(fileFormat, fileLocation!);

        // Assert
        Assert.Throws<IncorrectSettingsException>(Act);
    }

    [Fact]
    public void CreateFileWriter_EmptyFileLocation_ThrowsInvalidSettingsException()
    {
        // Arrange
        const string fileFormat = FileFormat.Csv;
        const string fileLocation = "";

        // Act
        void Act() => _fileWriterFactory.CreateFileWriter(fileFormat, fileLocation);

        // Assert
        Assert.Throws<IncorrectSettingsException>(Act);
    }
    
    [Fact]
    public void CreateFileWriter_InvalidFileLocation_ThrowsDirectoryNotFoundException()
    {
        // Arrange
        const string fileFormat = FileFormat.Csv;
        const string fileLocation = "./invalid./location.csv";

        // Act
        void Act() => _fileWriterFactory.CreateFileWriter(fileFormat, fileLocation);

        // Assert
        Assert.Throws<DirectoryNotFoundException>(Act);
    }
}