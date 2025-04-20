using RigCountDownloader.Domain.Models;

namespace RigCountDownloader.Domain.Interfaces
{
	public interface IDataConverter
	{
		IConvertedData ConvertData(Data data);
	}
}