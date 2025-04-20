namespace RigCountDownloader.Domain.Interfaces.Factories
{
	public interface IDataProcessorFactory
	{
		IDataProcessor CreateDataProcessor(IFileWriter fileWriter, IConvertedData data);
	}
}