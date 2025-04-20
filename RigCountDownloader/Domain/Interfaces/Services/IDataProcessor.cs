namespace RigCountDownloader.Domain.Interfaces.Services
{
	public interface IDataProcessor
	{
		Task ProcessAndSaveAsync(CancellationToken cancellationToken);
	}
}
