namespace RigCountDownloader
{
	public interface IFileService
	{
		Task WriteToFileAsync(Stream stream);
	}
}