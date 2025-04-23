namespace RigCountDownloader.Domain.Interfaces
{
    public interface IDataProcessor
    {
        Task ProcessAndSaveDataAsync(Options options, CancellationToken cancellationToken);
    }
}