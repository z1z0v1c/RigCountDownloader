using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using RigCountDownloader;

// Configure dependency injection
var serviceProvider = new ServiceCollection()
	.AddHttpClient()
	.AddTransient<Downloader>()
	.BuildServiceProvider();

// Resolve the Downloader instance
var downloader = serviceProvider.GetRequiredService<Downloader>();

HtmlDocument htmlDocument = await downloader.GetHtmlDocumentAsync();

Console.WriteLine(htmlDocument.DocumentNode.InnerHtml.ToString());
