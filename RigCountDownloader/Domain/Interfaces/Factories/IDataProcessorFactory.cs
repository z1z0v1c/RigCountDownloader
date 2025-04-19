namespace RigCountDownloader.FileConverters
{
	public interface IDataProcessorFactory
	{
		IDataProcessor CreateFileConverter(Data data);
	}
}