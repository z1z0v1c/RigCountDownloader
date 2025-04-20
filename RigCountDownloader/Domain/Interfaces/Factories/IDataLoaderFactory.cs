using RigCountDownloader.Domain.Models;

namespace RigCountDownloader.Domain.Interfaces.Factories;

public interface IDataLoaderFactory
{
    IDataLoader CreateDataLoader(Settings settings, CancellationToken cancellationToken); 
}