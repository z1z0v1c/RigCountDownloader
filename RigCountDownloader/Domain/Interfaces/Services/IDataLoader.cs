using RigCountDownloader.Domain.Models;

namespace RigCountDownloader.Domain.Interfaces.Services;

public interface IDataLoader
{
    Task<Data> LoadDataAsync(Uri uri, CancellationToken cancellationToken);
}