using RigCountDownloader.Domain.Models;

namespace RigCountDownloader.Domain.Interfaces.Services.Factories;

public interface IDataLoaderFactory
{
    IDataLoader CreateDataLoader(Settings settings, CancellationToken cancellationToken); 
}