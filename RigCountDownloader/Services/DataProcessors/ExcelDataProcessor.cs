using OfficeOpenXml;

namespace RigCountDownloader.Services.DataProcessors
{
    public abstract class ExcelDataProcessor(IFileWriter fileWriter, ExcelPackage excelPackage)
        : IDataProcessor
    {
        protected IFileWriter FileWriter { get; } = fileWriter;

        public ExcelPackage ExcelPackage { get; set; } = excelPackage;

        public abstract Task ProcessAndSaveDataAsync(Options options, CancellationToken cancellationToken = default);
    }
}