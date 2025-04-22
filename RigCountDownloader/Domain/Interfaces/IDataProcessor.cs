namespace RigCountDownloader.Domain.Interfaces
{
    public interface IDataProcessor
    {
        Task ProcessAndSaveDataAsync(CancellationToken cancellationToken);
    }
}