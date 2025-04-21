using RigCountDownloader.Domain.Models;

namespace RigCountDownloader.Domain.Interfaces
{
	public interface IDataFormater
	{
		FormatedData FormatData(DataStream dataStream);
	}
}