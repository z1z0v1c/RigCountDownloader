using HtmlAgilityPack;

namespace RigCountDownloader
{
	public interface IDownloadService
	{
		Task<HtmlDocument> GetHtmlDocumentAsync(string uri);

		Task DownloadFileAsync(HtmlDocument htmlDocument, string baseAddress, string fileName);
	}
}