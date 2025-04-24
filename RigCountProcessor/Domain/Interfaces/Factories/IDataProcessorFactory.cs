namespace RigCountProcessor.Domain.Interfaces.Factories;

public interface IDataProcessorFactory
{
	IDataProcessor CreateDataProcessor(IFileWriter fileWriter, string fileFormat, string fileName, object data);
}