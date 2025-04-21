using RigCountDownloader.Domain.Models;

namespace RigCountDownloader.Domain.Interfaces.Services;

public interface IDataLoader
{
    Task<Data> LoadDataAsync(string fileLocation, CancellationToken cancellationToken);
}