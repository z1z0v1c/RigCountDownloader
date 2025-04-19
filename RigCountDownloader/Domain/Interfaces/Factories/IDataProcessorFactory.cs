using RigCountDownloader.Domain.Interfaces.DataProcessors;

namespace RigCountDownloader.Domain.Interfaces.Factories
{
	public interface IDataProcessorFactory
	{
		IDataProcessor CreateDataProcessor(IConvertedData data);
	}
}