namespace RigCountProcessor.Tests.Services.Factories;

public class DataLoaderFactoryTests : TestFixture
{
    private readonly IDataLoaderFactory _dataLoaderFactory;

    public DataLoaderFactoryTests()
    {
        var mockHttpHandler = ServiceProvider.GetRequiredService<MockHttpMessageHandler>();
        _dataLoaderFactory = new DataLoaderFactory(mockHttpHandler.ToHttpClient());
    }

    [Fact]
    public void CreateHttpDataLoader_ValidSourceType_ReturnsCorrectResult()
    {
        // Arrange
        const string sourceType = SourceType.Http;
        
        // Act
        var dataLoader = _dataLoaderFactory.CreateDataLoader(sourceType);
        
        // Assert
        Assert.IsType<HttpDataLoader>(dataLoader);
    }
    
    [Fact]
    public void CreateHttpDataLoader_InvalidSourceType_ReturnsCorrectResult()
    {
        // Arrange
        const string sourceType = "wrong-type";
        
        // Act
        void Act() => _dataLoaderFactory.CreateDataLoader(sourceType);
        
        // Assert
        Assert.Throws<IncorrectSettingsException>(Act);
    }
}