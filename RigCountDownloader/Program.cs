using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RigCountDownloader;
using RigCountDownloader.FileConverters;
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
	.AddTransient<StreamDownloader>()
	.AddTransient<StreamProcessorFactory>()
	.AddTransient<IFileConverterFactory, FileConverterFactory>()
	.BuildServiceProvider();

// Resolve dependencies
ILogger logger = serviceProvider.GetRequiredService<ILogger>();
IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
StreamDownloader streamDownloader = serviceProvider.GetRequiredService<StreamDownloader>();
StreamProcessorFactory streamProcessorFactory = serviceProvider.GetRequiredService<StreamProcessorFactory>();

await new Application(logger, configuration, streamDownloader, streamProcessorFactory).RunAsync();
