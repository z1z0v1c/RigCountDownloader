namespace RigCountDownloader;

public interface IDataLoader
{
    Task<Data> LoadDataAsync(Uri uri);
}