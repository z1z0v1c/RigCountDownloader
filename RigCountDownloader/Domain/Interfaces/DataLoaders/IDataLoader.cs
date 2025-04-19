using RigCountDownloader.Domain.Models;

namespace RigCountDownloader.Domain.Interfaces.DataLoaders;

public interface IDataLoader
{
    Task<Data> LoadDataAsync(Uri uri);
}