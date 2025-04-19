using RigCountDownloader.Domain.Models;

namespace RigCountDownloader.Domain.Interfaces.DataConverters
{
	public interface IDataConverter
	{
		Task<IConvertedData> ConvertDataAsync(Data data);
	}
}