using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using RigCountDownloader;

// Configure dependency injection
ServiceProvider serviceProvider = new ServiceCollection()
	.AddHttpClient()
	.AddTransient<IDownloader, Downloader>()
	.BuildServiceProvider();

// Resolve the Downloader instance
IDownloader downloader = serviceProvider.GetRequiredService<IDownloader>();

HtmlDocument htmlDocument = await downloader.GetHtmlDocumentAsync();

Console.WriteLine(htmlDocument.DocumentNode.InnerHtml.ToString());
