namespace RigCountDownloader
{
	public interface IFileService
	{
		Task WriteToFileAsync(HttpContent content);
	}
}