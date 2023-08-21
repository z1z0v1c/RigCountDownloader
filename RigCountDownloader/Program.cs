using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using RigCountDownloader;

// Configure dependency injection
ServiceProvider serviceProvider = new ServiceCollection()
	.AddHttpClient()
	.AddTransient<IDownloadService, DownloadService>()
	.AddScoped<IFileService, ExcelFileService>()
	.BuildServiceProvider();

// Resolve the Downloader instance
IDownloadService downloader = serviceProvider.GetRequiredService<IDownloadService>();

HtmlDocument htmlDocument = await downloader.GetHtmlDocumentAsync();

await downloader.DownloadFileAsync(htmlDocument);
