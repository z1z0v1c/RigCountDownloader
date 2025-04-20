using RigCountDownloader.Domain.Models;

namespace RigCountDownloader.Domain.Interfaces.Services
{
	public interface IDataConverter
	{
		IConvertedData ConvertData(Data data);
	}
}