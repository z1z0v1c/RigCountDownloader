using OfficeOpenXml;
using RigCountDownloader.Domain.Interfaces;
using Serilog;

namespace RigCountDownloader.Services.DataProcessors
{
    public abstract class ExcelDataProcessor(ILogger logger, IFileWriter fileWriter, ExcelPackage excelPackage)
        : IDataProcessor
    {
        protected ILogger Logger { get; } = logger;
        protected IFileWriter FileWriter { get; } = fileWriter;
        public ExcelPackage ExcelPackage { get; set; } = excelPackage;

        public abstract Task ProcessAndSaveAsync(CancellationToken cancellationToken = default);
    }
}