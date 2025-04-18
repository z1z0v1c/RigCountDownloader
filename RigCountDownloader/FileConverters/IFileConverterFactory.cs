namespace RigCountDownloader.FileConverters
{
	public interface IFileConverterFactory
	{
		IFileConverter CreateFileConverter(Response response);
	}
}