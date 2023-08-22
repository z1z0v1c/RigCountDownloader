using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RigCountDownloader;

var configuration = new ConfigurationBuilder()
				.AddJsonFile(Directory.GetCurrentDirectory() + "..\\..\\..\\..\\appsettings.json")
				.Build();

// Configure dependency injection
ServiceProvider serviceProvider = new ServiceCollection()
	.AddHttpClient()
	.AddSingleton<IConfiguration>(configuration)
	.AddTransient<IDownloadService, DownloadService>()
	.AddScoped<IFileService, ExcelFileService>()
	.BuildServiceProvider();

// Resolve the Downloader instance
IDownloadService downloader = serviceProvider.GetRequiredService<IDownloadService>();

HtmlDocument htmlDocument = await downloader.GetHtmlDocumentAsync();

await downloader.DownloadFileAsync(htmlDocument);
