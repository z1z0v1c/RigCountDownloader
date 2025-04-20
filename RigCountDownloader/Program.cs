using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RigCountDownloader.Application;
using RigCountDownloader.Domain.Interfaces.Factories;
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
	// Register Factories
	.AddSingleton<IDataLoaderFactory, DataLoaderFactory>()
	.AddSingleton<IDataConverterFactory, DataConverterFactory>()
	.AddSingleton<IDataProcessorFactory, DataProcessorFactory>()
	// Register Pipeline
	.AddSingleton<Pipeline>()
	// Register Application
	.AddSingleton<Application>()
	.BuildServiceProvider();

// Resolve and run Application
await serviceProvider.GetRequiredService<Application>().RunAsync();
