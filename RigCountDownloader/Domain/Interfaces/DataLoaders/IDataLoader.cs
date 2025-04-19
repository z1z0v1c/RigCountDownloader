namespace RigCountDownloader;

public interface IDataLoader
{
    Task<Response> DownloadFileAsStreamAsync(Uri uri);
}