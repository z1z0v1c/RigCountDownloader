using Microsoft.Extensions.Configuration;

namespace RigCountProcessor.Tests;

public class TestFixture
{
    protected IServiceProvider ServiceProvider { get; private set; }

    protected TestFixture()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        // Configure dependencies
        var services = new ServiceCollection()
            .AddSingleton(provider => Substitute.For<ILogger>())
            .AddSingleton(provider => Substitute.For<IConfiguration>())
            .AddSingleton<MockHttpMessageHandler>()
            .AddSingleton<IDataProcessorFactory, DataProcessorFactory>()
            .AddSingleton<IFileWriterFactory, FileWriterFactory>()
            .AddSingleton<RigCountDataProcessor>();

        ServiceProvider = services.BuildServiceProvider();
    }
}