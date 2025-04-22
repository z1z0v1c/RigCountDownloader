namespace RigCountDownloader.Domain.Interfaces.Factories;

public interface IDataLoaderFactory
{
    IDataLoader CreateDataLoader(string sourceType);
}