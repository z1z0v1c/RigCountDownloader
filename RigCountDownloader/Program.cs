using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RigCountDownloader;
using RigCountDownloader.Application;
using RigCountDownloader.Domain.Interfaces.DataConverters;
using RigCountDownloader.Domain.Interfaces.DataLoaders;
using RigCountDownloader.Domain.Interfaces.DataProcessors;
using RigCountDownloader.Domain.Interfaces.Factories;
using RigCountDownloader.Services.DataConverters;
using RigCountDownloader.Services.DataLoaders;
using RigCountDownloader.Services.DataProcessors;
using RigCountDownloader.Services.Factories;
using Serilog;

await using var log = new LoggerConfiguration()
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
	// Register factories
	.AddTransient<IDataLoaderFactory, DataLoaderFactory>()
	.AddTransient<IDataConverterFactory, DataConverterFactory>()
	.AddTransient<IDataProcessorFactory, DataProcessorFactory>()
	// Register pipeline
	.AddTransient<Pipeline>()
	.BuildServiceProvider();

// Resolve dependencies
ILogger logger = serviceProvider.GetRequiredService<ILogger>();
IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
Pipeline pipeline = serviceProvider.GetRequiredService<Pipeline>();

await new Application(logger, configuration, pipeline).RunAsync();
