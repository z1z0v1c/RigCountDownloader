namespace RigCountDownloader.Domain.Interfaces
{
	public interface IDataProcessor
	{
		Task ProcessAndSaveAsync(CancellationToken cancellationToken);
	}
}
