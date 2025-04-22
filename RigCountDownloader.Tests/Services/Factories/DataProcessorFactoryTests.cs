using RigCountDownloader.Tests.Mocks;

namespace RigCountDownloader.Tests.Services.Factories
{
    public class DataProcessorFactoryTests : TestFixture
    {
        private readonly IDataProcessorFactory _dataProcessorFactory;

        public DataProcessorFactoryTests()
        {
            _dataProcessorFactory = ServiceProvider.GetRequiredService<IDataProcessorFactory>();
        }

        [Fact]
        public void CreateXlsxDataProcessor_ValidSettings_ReturnsCorrectResult()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            using var fileWriter = new MockFileWriter(memoryStream);
            var data = new ExcelPackage(memoryStream);

            // Act
            var dataProcessor =
                _dataProcessorFactory.CreateDataProcessor(fileWriter, Context.RigCount, FileFormat.Xlsx, data);

            // Assert
            Assert.IsType<RigCountDataProcessor>(dataProcessor);
        }

        [Fact]
        public void CreateDataProcessor_InvalidFileFormat_ThrowsIncorrectSettingsException()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            using var fileWriter = new MockFileWriter(memoryStream);
            var data = new ExcelPackage(memoryStream);

            // Act
            void Act() => _dataProcessorFactory.CreateDataProcessor(fileWriter, Context.RigCount, "pdb", data);

            // Assert
            Assert.Throws<IncorrectSettingsException>(Act);
        }

        [Fact]
        public void CreateDataProcessor_InvalidContext_ThrowsIncorrectSettingsException()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            using var fileWriter = new MockFileWriter(memoryStream);
            var data = new ExcelPackage(memoryStream);

            // Act
            void Act() => _dataProcessorFactory.CreateDataProcessor(fileWriter, "Rig Rate", FileFormat.Xlsx, data);

            // Assert
            Assert.Throws<IncorrectSettingsException>(Act);
        }
    }
}