namespace RigCountDownloader.Domain.Interfaces;

public interface IDataLoader
{
    Task<DataStream> LoadDataAsync(string fileLocation, CancellationToken cancellationToken);
}