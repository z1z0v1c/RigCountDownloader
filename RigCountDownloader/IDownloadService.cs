namespace RigCountDownloader
{
	public interface IDownloadService
	{
		Task<Stream> DownloadFileAsStreamAsync();
	}
}