namespace RigCountDownloader
{
	public interface IDownloadService
	{
		Task DownloadFileAsync(string uri, string fileName);

		Task DownloadFileAsync(Uri uri, string fileName);
	}
}