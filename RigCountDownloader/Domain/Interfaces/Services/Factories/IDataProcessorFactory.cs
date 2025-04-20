namespace RigCountDownloader.Domain.Interfaces.Services.Factories
{
	public interface IDataProcessorFactory
	{
		IDataProcessor CreateDataProcessor(IFileWriter fileWriter, IConvertedData data);
	}
}