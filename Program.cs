using HtmlAgilityPack;
using RigCountDownloader;

Downloader downloader = new();

HtmlDocument htmlDocument = await downloader.GetHtmlDocumentAsync();

Console.WriteLine(htmlDocument.DocumentNode.InnerHtml.ToString());
