using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RigCountDownloader;
using RigCountDownloader.FileConverters;
using RigCountDownloader.FileModificators;
using RigCountDownloader.StreamProcessors;

var configurationRoot = new ConfigurationBuilder()
				.AddJsonFile(Directory.GetCurrentDirectory() + "..\\..\\..\\..\\appsettings.json")
				.Build();

// Configure dependency injection
ServiceProvider serviceProvider = new ServiceCollection()
	.AddHttpClient()
	.AddSingleton<IConfiguration>(configurationRoot)
	.AddTransient<StreamDownloader>()
	.AddTransient<StreamProcessorFactory>()
	.AddTransient<FileModificatorFactory>()
	.AddTransient<FileConverterFactory>()
	.BuildServiceProvider();

// Resolve dependencies
StreamDownloader streamDownloader = serviceProvider.GetRequiredService<StreamDownloader>();
StreamProcessorFactory streamProcessorFactory = serviceProvider.GetRequiredService<StreamProcessorFactory>();

await new Application(streamDownloader, streamProcessorFactory).RunAsync();
