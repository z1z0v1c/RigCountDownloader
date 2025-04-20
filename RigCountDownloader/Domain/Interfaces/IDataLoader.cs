using RigCountDownloader.Domain.Models;

namespace RigCountDownloader.Domain.Interfaces;

public interface IDataLoader
{
    Task<Data> LoadDataAsync(Uri uri, CancellationToken cancellationToken);
}