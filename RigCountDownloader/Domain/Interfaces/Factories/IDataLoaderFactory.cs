namespace RigCountDownloader.Domain.Interfaces.Services.Factories;

public interface IDataLoaderFactory
{
    IDataLoader CreateDataLoader(string sourceType);
}