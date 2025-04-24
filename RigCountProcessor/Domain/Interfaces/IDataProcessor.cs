namespace RigCountProcessor.Domain.Interfaces;

public interface IDataProcessor
{
    Task ProcessAndSaveDataAsync(Options options, CancellationToken cancellationToken);
}