using RigCountProcessor.Tests.Mocks;

namespace RigCountProcessor.Tests.Services.DataProcessors;

public class RigCountDataProcessorTests : TestFixture
{
    private IFileWriter? _fileWriter;
    private RigCountDataProcessor? _dataProcessor;

    [Fact]
    public async Task ProcessAndSaveAsync_ValidData_CaseOne()
    {
        // Arrange
        var options = new Options(2023, 2);

        using var data = new MemoryStream();
        await using (var fileStream = File.OpenRead("../../../TestData/Valid_Rig_Count_Data.xlsx"))
        {
            await fileStream.CopyToAsync(data);
        }

        using var actual = new MemoryStream();
        _fileWriter = new MockFileWriter(actual);

        _dataProcessor = new RigCountDataProcessor(_fileWriter, new ExcelPackage(data));

        using var expected = new MemoryStream();
        await using (var fileStream = File.OpenRead("../../../TestData/Rig_Count_Case_1.csv"))
        {
            await fileStream.CopyToAsync(expected);
        }

        // Act
        await _dataProcessor.ProcessAndSaveDataAsync(options);

        // Assert
        Assert.Equal(expected.ToArray(), (actual.ToArray()));
    }

    [Fact]
    public async Task ProcessAndSaveAsync_ValidData_CaseTwo()
    {
        // Arrange
        var options = new Options(2021, 4);

        using var data = new MemoryStream();
        await using (var fileStream = File.OpenRead("../../../TestData/Valid_Rig_Count_Data.xlsx"))
        {
            await fileStream.CopyToAsync(data);
        }

        using var actual = new MemoryStream();
        _fileWriter = new MockFileWriter(actual);

        _dataProcessor = new RigCountDataProcessor(_fileWriter, new ExcelPackage(data));

        using var expected = new MemoryStream();
        await using (var fileStream = File.OpenRead("../../../TestData/Rig_Count_Case_2.csv"))
        {
            await fileStream.CopyToAsync(expected);
        }

        // Act
        await _dataProcessor.ProcessAndSaveDataAsync(options);

        // Assert
        Assert.Equal(expected.ToArray(), (actual.ToArray()));
    }

    [Fact]
    public async Task ProcessAndSaveAsync_InvalidData_ThrowsInvalidDataException()
    {
        // Arrange
        using var data = new MemoryStream();
        await using (var fileStream = File.OpenRead("../../../TestData/Invalid_Rig_Count_Data.xlsx"))
        {
            await fileStream.CopyToAsync(data);
        }

        _fileWriter = new MockFileWriter(new MemoryStream());
        _dataProcessor = new RigCountDataProcessor(_fileWriter, new ExcelPackage(data));

        // Act
        async Task Act() => await _dataProcessor.ProcessAndSaveDataAsync(new Options());

        // Assert
        await Assert.ThrowsAsync<InvalidDataException>(Act);
    }

    [Fact]
    public async Task ProcessAndSaveAsync_EmptyWorksheet_ThrowsInvalidDataException()
    {
        // Arrange
        _fileWriter = new MockFileWriter(new MemoryStream());
        _dataProcessor = new RigCountDataProcessor(_fileWriter, new ExcelPackage());

        _dataProcessor.ExcelPackage.Workbook.Worksheets.Add("EmptySheet");

        // Act
        async Task Act() => await _dataProcessor.ProcessAndSaveDataAsync(new Options());

        // Assert
        await Assert.ThrowsAsync<InvalidDataException>(Act);
    }

    [Fact]
    public async Task ProcessAndSaveAsync_NoData_ThrowsInvalidDataException()
    {
        // Arrange
        _fileWriter = new MockFileWriter(new MemoryStream());
        _dataProcessor = new RigCountDataProcessor(_fileWriter, new ExcelPackage());

        using var memoryStream = new MemoryStream();
        using var package = new ExcelPackage();

        _dataProcessor.ExcelPackage = package;

        // Act
        async Task Act() => await _dataProcessor.ProcessAndSaveDataAsync(new Options());

        // Assert
        await Assert.ThrowsAsync<InvalidDataException>(Act);
    }

    // TODO add more tests
}