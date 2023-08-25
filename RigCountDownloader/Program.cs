using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RigCountDownloader;

var configurationRoot = new ConfigurationBuilder()
				.AddJsonFile(Directory.GetCurrentDirectory() + "..\\..\\..\\..\\appsettings.json")
				.Build();

// Configure dependency injection
ServiceProvider serviceProvider = new ServiceCollection()
	.AddHttpClient()
	.AddSingleton<IConfiguration>(configurationRoot)
	.AddTransient<IDownloadService, DownloadService>()
	.AddScoped<FileServiceFactory>()
	.BuildServiceProvider();

// Resolve dependencies
IDownloadService downloadService = serviceProvider.GetRequiredService<IDownloadService>();
FileServiceFactory fileServiceFactory = serviceProvider.GetRequiredService<FileServiceFactory>();

await new Application(downloadService, fileServiceFactory).RunAsync();
