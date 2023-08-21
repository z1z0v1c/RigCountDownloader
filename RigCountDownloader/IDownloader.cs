using HtmlAgilityPack;

namespace RigCountDownloader
{
	public interface IDownloader
	{
		Task<HtmlDocument> GetHtmlDocumentAsync();
	}
}