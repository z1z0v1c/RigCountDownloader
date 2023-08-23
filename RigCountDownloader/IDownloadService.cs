namespace RigCountDownloader
{
	public interface IDownloadService
	{
		Task<Stream> DownloadFileAsync(string uri, string fileName);

		Task<Stream> DownloadFileAsync(Uri uri, string fileName);
	}
}