namespace RigCountDownloader.StreamProcessors
{
	public interface IDataConverter
	{
		Task ConvertDataAsync(Stream stream);
	}
}