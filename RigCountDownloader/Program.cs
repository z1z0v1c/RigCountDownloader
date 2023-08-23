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
	.AddScoped<IFileService, ExcelFileService>()
	.BuildServiceProvider();

// Resolve dependencies
IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
IDownloadService downloadService = serviceProvider.GetRequiredService<IDownloadService>();
IFileService fileService = serviceProvider.GetRequiredService<IFileService>();

await new Application(configuration, downloadService, fileService).RunAsync();
