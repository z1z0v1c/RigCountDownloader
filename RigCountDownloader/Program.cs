using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RigCountDownloader.Application;
using RigCountDownloader.Domain.Interfaces.Factories;
using RigCountDownloader.Domain.Interfaces.Services.Factories;
using RigCountDownloader.Services.Factories;
using Serilog;

await using var logger = new LoggerConfiguration()
	.WriteTo.Console()
	.WriteTo.File("./log.txt")
	.CreateLogger();

var configurationRoot = new ConfigurationBuilder()
	.AddJsonFile(Directory.GetCurrentDirectory() + "./appsettings.json")
	.Build();

// Configure dependency injection
var serviceProvider = new ServiceCollection()
	.AddHttpClient()
	.AddSingleton<ILogger>(logger)
	.AddSingleton<IConfiguration>(configurationRoot)
	// Register Factories
	.AddSingleton<IDataLoaderFactory, DataLoaderFactory>()
	.AddSingleton<IDataFormaterFactory, DataFormaterFactory>()
	.AddSingleton<IDataProcessorFactory, DataProcessorFactory>()
	.AddSingleton<IFileWriterFactory, FileWriterFactory>()
	// Register Pipeline
	.AddSingleton<Pipeline>()
	// Register Application
	.AddSingleton<Application>()
	.BuildServiceProvider();

// Resolve and run Application
await serviceProvider.GetRequiredService<Application>().RunAsync();
