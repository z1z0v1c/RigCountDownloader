namespace RigCountDownloader.FileConverters
{
	public interface IFileConverter
	{
		// Separate into two methods/interfaces?
		Task ConvertAndSaveAsync();
	}
}
