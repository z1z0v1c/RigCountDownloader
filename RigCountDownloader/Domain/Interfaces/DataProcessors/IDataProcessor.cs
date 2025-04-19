namespace RigCountDownloader.Domain.Interfaces.DataProcessors
{
	public interface IDataProcessor
	{
		Task ProcessAndSaveAsync();
	}
}
