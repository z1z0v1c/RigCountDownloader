using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RigCountDownloader;
using RigCountDownloader.Application;
using RigCountDownloader.FileConverters;
using RigCountDownloader.Services.Factories;
using RigCountDownloader.StreamProcessors;
using Serilog;

using var log = new LoggerConfiguration()
	.WriteTo.Console()
	.WriteTo.File("./log.txt")
	.CreateLogger();

var configurationRoot = new ConfigurationBuilder()
	.AddJsonFile(Directory.GetCurrentDirectory() + "./appsettings.json")
	.Build();

// Configure dependency injection
ServiceProvider serviceProvider = new ServiceCollection()
	.AddHttpClient()
	.AddSingleton<ILogger>(log)
	.AddSingleton<IConfiguration>(configurationRoot)
	.AddTransient<HttpDataLoader>()
	.AddTransient<DataConverterFactory>()
	.AddTransient<IDataProcessorFactory, DataProcessorFactory>()
	.BuildServiceProvider();

// Resolve dependencies
ILogger logger = serviceProvider.GetRequiredService<ILogger>();
IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
HttpDataLoader httpDataLoader = serviceProvider.GetRequiredService<HttpDataLoader>();
DataConverterFactory dataConverterFactory = serviceProvider.GetRequiredService<DataConverterFactory>();

await new Application(logger, configuration, httpDataLoader, dataConverterFactory).RunAsync();
