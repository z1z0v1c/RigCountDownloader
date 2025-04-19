namespace RigCountDownloader;

public interface IDataLoader
{
    Task<Response> LoadDataAsync(Uri uri);
}