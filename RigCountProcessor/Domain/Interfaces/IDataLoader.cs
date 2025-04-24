namespace RigCountProcessor.Domain.Interfaces;

public interface IDataLoader
{
    Task<DataStream> LoadDataAsync(string fileLocation, CancellationToken cancellationToken);
}