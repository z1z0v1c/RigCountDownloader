namespace RigCountDownloader.StreamProcessors
{
	public interface IStreamProcessor
	{
		Task ProcessStreamAsync(Stream stream);
	}
}