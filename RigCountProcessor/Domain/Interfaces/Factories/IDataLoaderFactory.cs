namespace RigCountProcessor.Domain.Interfaces.Factories;

public interface IDataLoaderFactory
{
    IDataLoader CreateDataLoader(string sourceType);
}