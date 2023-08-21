using HtmlAgilityPack;

namespace RigCountDownloader
{
	public interface IDownloadService
	{
		Task<HtmlDocument> GetHtmlDocumentAsync();

		Task DownloadFileAsync(HtmlDocument htmlDocument);
	}
}