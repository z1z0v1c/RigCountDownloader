namespace RigCountProcessor.Domain.Interfaces;

public interface IDataFormatter
{
    FormattedData FormatData(DataStream dataStream);
}