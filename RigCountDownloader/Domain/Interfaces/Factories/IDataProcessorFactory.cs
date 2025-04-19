namespace RigCountDownloader.FileConverters
{
	public interface IDataProcessorFactory
	{
		IDataProcessor CreateFileConverter(Response response);
	}
}